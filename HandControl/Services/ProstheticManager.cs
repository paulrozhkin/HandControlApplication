// --------------------------------------------------------------------------------------
// <copyright file = "ProstheticManager.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using HandControl.Model;
using HandControl.Model.Enums;
using HandControl.Services.IODevice;
using SaveGestureProtobuf = HandControl.Model.Protobuf.SaveGesture;
using GestureProtobuf = HandControl.Model.Protobuf.Gesture;
using GetGesturesProtobuf = HandControl.Model.Protobuf.GetGestures;

namespace HandControl.Services
{
    /// <summary>
    ///     Класс, предоставляющий API для управления протезом руки.
    ///     Является имплементацией паттерна Singleton.
    ///     \version 1.0
    ///     \date Январь 2019 года
    ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class ProstheticManager
    {
        #region Constuctors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProstheticManager" /> class.
        /// </summary>
        private ProstheticManager()
        {
            _connectedDevices = new DeviceBluetooth();
            TelemetryReceived = _connectedDevices.TelemetryPackages.Select(x => new Telemetry());
            TelemetryReceived.Subscribe(TelemetryReceivedHandler);
        }

        #endregion

        #region Fields

        /// <summary>
        ///     Обеспечивает потокобезопасное извлечение instance.
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        ///     Хранит одиночный экземпляр класса <see cref="ProstheticManager" />.
        /// </summary>
        private static ProstheticManager _instance;

        /// <summary>
        ///     Экземпляр, выполняющий соедниение с устройством протеза.
        /// </summary>
        private readonly IIoDevice _connectedDevices;
        #endregion

        #region Events

#pragma warning disable
        /// <summary>
        ///     Имплементация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Enums
        public IObservable<bool> IsConnectionChanged => _connectedDevices.IsConnectedStatusChanged;

        public IObservable<Telemetry> TelemetryReceived { get; }
        #endregion

        #region Methods

        /// <summary>
        ///     Получить единичный экземпляр класса <see cref="ProstheticManager" />.
        /// </summary>
        /// <returns>Единичный экземпляр класса <see cref="ProstheticManager" />.</returns>
        public static ProstheticManager GetInstance()
        {
            if (_instance == null)
                lock (SyncRoot)
                {
                    if (_instance == null) _instance = new ProstheticManager();
                }

            return _instance;
        }

        public async Task<IEnumerable<GestureModel>> GetGestures()
        {
            var payload = await _connectedDevices.SendToDevice(CommandType.GetGestures);
            var gestures = GetGesturesProtobuf.Parser.ParseFrom(payload);
            return null;
        }

        /// <summary>
        ///     Сохранение или обновление жеста на протезе.
        /// </summary>
        /// <param name="gestureList">Сохраняемый жест.</param>
        public async Task SaveGestures(GestureModel gesture)
        {
            var saveGesture = new SaveGestureProtobuf()
            {
                TimeSync = DateTimeOffset.Now.ToUnixTimeSeconds(),
                Gesture = new GestureProtobuf()
                {
                }
            };
            
            saveGesture.ToByteArray();
            await _connectedDevices.SendToDevice(CommandType.SaveGestures, null);
        }

        /// <summary>
        ///     Исполнение жеста на протезе.
        /// </summary>
        /// <param name="gesture">Исполняемый жест.</param>
        public void ExecuteTheGesture(GestureModel gesture)
        {
            //// TODO: вернуть.
            var dataField = new List<byte> {(byte) CommandType.PerformGestureRaw};
            ////dataField.AddRange(gesture.BinaryDate.ToList<byte>());
            _connectedDevices.SendToDevice(CommandType.PerformGestureRaw, null);
        }

        /// <summary>
        ///     Исполнение жеста на протезе по id.
        /// </summary>
        /// <param name="gestureId">Id жеста.</param>
        public void ExecuteTheGesture(string gestureId)
        {
            _connectedDevices.SendToDevice(CommandType.PerformGestureId, null);
        }

        public void Connect()
        {
            _connectedDevices.ConnectDevice();
        }

        private void TelemetryReceivedHandler(Telemetry telemetry)
        {
            var test = 0;
        }
        #endregion
    }
}