// --------------------------------------------------------------------------------------
// <copyright file = "ProstheticManager.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
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
    public class ProstheticManager : IProstheticManager
    {
        #region Fields

        private readonly IProstheticConnector _prostheticConnector;
        private GetSettingsDto _settings;

        #endregion

        #region Constuctors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProstheticManager" /> class.
        /// </summary>
        public ProstheticManager(IGestureService gestureService, IProstheticConnector prostheticConnector,
            IMioPatternsService mioPatternsService)
        {
            GestureService = gestureService ?? throw new ArgumentNullException(nameof(gestureService));
            _prostheticConnector = prostheticConnector ?? throw new ArgumentNullException(nameof(prostheticConnector));
            MioPatternsService = mioPatternsService ?? throw new ArgumentNullException(nameof(mioPatternsService));
            TelemetryReceived.Subscribe(TelemetryReceivedHandler);
        }

        #endregion

        #region Properties

        public IObservable<bool> IsConnectionChanged => _prostheticConnector.IsConnectionChanged;

        public IObservable<TelemetryDto> TelemetryReceived => _prostheticConnector.TelemetryReceived;

        public IGestureService GestureService { get; }

        public IMioPatternsService MioPatternsService { get; }

        #endregion

        #region Methods

        public async Task ConnectAsync()
        {
            await _prostheticConnector.ConnectAsync().ConfigureAwait(false);
            _settings = await _prostheticConnector.GetSettingsAsync().ConfigureAwait(false);

            var telemetryDto = await _prostheticConnector.GetTelemetryAsync().ConfigureAwait(false);
            if (telemetryDto.LastTimeSync != GestureService.LastTimeSync)
            {
                await GestureService.SyncGesturesAsync().ConfigureAwait(false);
            }

            await MioPatternsService.GetMioPatternsAsync().ConfigureAwait(false);
        }

        private async void TelemetryReceivedHandler(TelemetryDto telemetryDto)
        {
        }

        #endregion
    }
}