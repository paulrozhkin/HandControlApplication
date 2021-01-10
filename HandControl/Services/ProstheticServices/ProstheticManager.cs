// --------------------------------------------------------------------------------------
// <copyright file = "ProstheticManager.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using HandControl.Model.Dto;

namespace HandControl.Services.ProstheticServices
{
    /// <summary>
    ///     Класс, предоставляющий сервис для управления протезом руки.
    ///     \version 1.0
    ///     \date Январь 2019 года
    ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class ProstheticManager: IProstheticManager
    {
        #region Fields

        private readonly IProstheticConnector _prostheticConnector;
        private GetSettingsDto _settings;
        private bool _isSynchronizeGestures;
        private SemaphoreSlim _synchronizeSemaphoreSlim = new SemaphoreSlim(1);
        #endregion

        #region Constuctors
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProstheticManager" /> class.
        /// </summary>
        public ProstheticManager(IGestureService gestureService, IProstheticConnector prostheticConnector)
        {
            GestureService = gestureService ?? throw new ArgumentNullException(nameof(gestureService));
            _prostheticConnector = prostheticConnector ?? throw new ArgumentNullException(nameof(prostheticConnector));
            TelemetryReceived.Subscribe(TelemetryReceivedHandler);
        }

        #endregion

        #region Properties
        public IObservable<bool> IsConnectionChanged => _prostheticConnector.IsConnectionChanged;

        public IObservable<TelemetryDto> TelemetryReceived => _prostheticConnector.TelemetryReceived;

        public IGestureService GestureService { get; }

        #endregion

        #region Methods
        public async Task ConnectAsync()
        {
            await _prostheticConnector.ConnectAsync();
            _settings = await _prostheticConnector.GetSettingsAsync();
        }

        private async void TelemetryReceivedHandler(TelemetryDto telemetryDto)
        {
            // TODO: сделать на основе сохранения информации о синхронизации, иначе есть большой шанс синхронизации после каждого добавления/удаления жеста.
            if (telemetryDto.LastTimeSync != GestureService.LastTimeSync && _isSynchronizeGestures == false)
            {
                _isSynchronizeGestures = true;
                
                if (_synchronizeSemaphoreSlim.CurrentCount == 1)
                {
                    await _synchronizeSemaphoreSlim.WaitAsync();

                    if (_isSynchronizeGestures)
                    {
                        await GestureService.SyncGesturesAsync();
                        _isSynchronizeGestures = false;
                        _synchronizeSemaphoreSlim.Release();
                    }
                }
            }
        }
        #endregion
    }
}