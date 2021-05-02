// --------------------------------------------------------------------------------------
// <copyright file = "IODeviceBluetooth.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using HandControl.Model;
using HandControl.Model.BluetoothDto;
using HandControl.Model.Enums;
using InTheHand.Net.Bluetooth;
using Newtonsoft.Json;

namespace HandControl.Services.IODevice.Bluetooth
{
    /// <summary>
    ///     Класса имплементирующий интерфейс IIoDevice.
    ///     Выполняет ввод-вывод данных через Bluetooth.
    ///     \brief Ввод-вывод данных через Bluetooth порт.
    ///     \version 1.0
    ///     \date Апрель 2019 года
    ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class DeviceBluetooth : IIoDevice, IDisposable
    {
        #region Fields

        private bool _isConnectedStatusChanged;
        private readonly Subject<bool> _isConnectedStatusChangedSubject = new Subject<bool>();
        #endregion


        #region Constructors

        private readonly IBluetoothService _service;
        private readonly BluetoothInfo _bluetoothInfo;
        private readonly ProtocolParser _protocolParser = new ProtocolParser();
        private readonly SemaphoreSlim _semaphoreSendPackage = new SemaphoreSlim(1);
        private Device _device;
        private TaskCompletionSource<byte[]> _packageResponseCompletionSource;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeviceBluetooth" /> class.
        /// </summary>
        public DeviceBluetooth()
        {
            _service = new BluetoothService();
            _bluetoothInfo = BluetoothInfo.InfoLoad();

            _service.ReceivedDataObservable.Subscribe(_protocolParser.Update);
            _protocolParser.ReceivedPackagesObservable.Where(x => x.Command != CommandType.Telemetry)
                .Subscribe(DataReceivedHandler);

            IsConnectedStatusChanged = _isConnectedStatusChangedSubject.AsObservable();
        }

        #endregion

        #region Properties

        public IObservable<bool> IsConnectedStatusChanged { get; }

        public IObservable<PackageDto> TelemetryPackages => _protocolParser.ReceivedPackagesObservable.Where(x => x.Command == CommandType.Telemetry);

        /// <summary>
        ///     Gets a value indicating whether состояние подключения протеза руки по Bluetooth.
        /// </summary>
        private bool IsBluetoothConnected {
            get => _isConnectedStatusChanged;
            set
            {
                if (value != _isConnectedStatusChanged)
                {
                    _isConnectedStatusChanged = value;
                    _isConnectedStatusChangedSubject.OnNext(value);
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        ///     Имплементация интерфейса IDisposable.
        ///     Выполняет закрытие соединения Bluetooth.
        /// </summary>
        public void Dispose()
        {
            _service.Dispose();
        }

        public Task<byte[]> SendToDeviceAsync(CommandType command)
        {
            return SendToDeviceAsync(command, null);
        }

        public async Task<byte[]> SendToDeviceAsync(CommandType command, byte[] payload)
        {
            await _semaphoreSendPackage.WaitAsync();

            try
            {
                if (!IsBluetoothConnected)
                {
                    throw new InvalidOperationException("Device not connected");
                }

                var package = _protocolParser.CreatePackage(command, payload);
                await _service.SendAsync(package);

                _packageResponseCompletionSource = new TaskCompletionSource<byte[]>();
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(10));

                using (cancellationTokenSource.Token.Register(() =>
                {
                    // this callback will be executed when token is cancelled
                    _packageResponseCompletionSource.TrySetCanceled();
                }))
                {
                    var response = await _packageResponseCompletionSource.Task;
                    return response;
                }
            }
            finally
            {
                _semaphoreSendPackage.Release();
            }
        }

        public async Task ConnectDeviceAsync()
        {
            while (!IsBluetoothConnected)
            {
                if (BluetoothRadio.IsSupported.Equals(true))
                {
                    var listDevices = await _service.GetAuthenticDevicesAsync();

                    foreach (var device in listDevices)
                        if (device.DeviceName.Equals(_bluetoothInfo.NameDevice))
                        {
                            if (!device.DeviceInfo.Authenticated)
                            {
                                var password = "hcaccess";
                                if (!BluetoothSecurity.PairRequest(device.DeviceInfo.DeviceAddress, password))
                                    throw new IOException("Bluetooth pair error. Name: " + device.DeviceName +
                                                          ", pass: " + password);
                            }

                            _device = device;

                            var status = await _service.ConnectAsync(device);

                            if (status)
                            {
                                IsBluetoothConnected = true;
                                var threadCheck = new Thread(ConnectionCheckerAsync);
                                threadCheck.Start();
                            }

                            break;
                        }
                }

                await Task.Delay(1000);
            }
        }

        private async void ConnectionCheckerAsync()
        {
            while (true)
            {
                var listDevices = await _service.GetAuthenticDevicesAsync();

                foreach (var device in listDevices)
                    if (device.DeviceName.Equals(_bluetoothInfo.NameDevice))
                        if (!device.IsConnected)
                        {
                            _service.Disconnect();
                            IsBluetoothConnected = false;
                            _ = ConnectDeviceAsync();
                            Thread.CurrentThread.Interrupt();
                        }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Thread check interrupt");
                    break;
                }
            }
        }

        public void DisconnectDevice()
        {
            _service.Disconnect();
        }

        private void DataReceivedHandler(PackageDto package)
        {
            if (_packageResponseCompletionSource == null)
            {
                throw new InvalidOperationException("The package was not expected to be received.");
            }

            if (package.Command == CommandType.Error)
            {
                _packageResponseCompletionSource.SetException(new Exception("Prosthetic error code"));
            }

            if (package.Crc != package.ReceivedCrc)
            {
                _packageResponseCompletionSource.SetException(new Exception("Crc not equals"));
            }

            _packageResponseCompletionSource.SetResult(package.Payload);
        }

        #endregion

        #region Classes

        /// <summary>
        ///     Класса экземпляры которого содержат информацию о Bluetooth устройства.
        ///     Выполняет загрузку и сохранение из файловой системы экземпляров.
        ///     \brief Информация о Bluetooth устройстве.
        ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
        /// </summary>
        private class BluetoothInfo
        {
            #region Properties

            /// <summary>
            ///     Gets or sets имя Bluetooth устройства протеза руки.
            /// </summary>
            [JsonProperty(PropertyName = "Name")]
            public string NameDevice { get; set; }

            /// <summary>
            ///     Gets or sets пароль Bluetooth устройства протеза руки.
            /// </summary>
            [JsonProperty(PropertyName = "Password")]
            public string PasswordDevice { get; set; }

            #endregion

            #region Methods

            /// <summary>
            ///     Загрузка информации из файловой системы о параметрах конфигурации COM устройств.
            /// </summary>
            /// <returns>Экземпляр <see cref="BluetoothInfo" />.</returns>
            public static BluetoothInfo InfoLoad()
            {
                var info =
                    (BluetoothInfo) JsonSerDer.LoadObject<BluetoothInfo>(PathManager.IoDevicePath("Bluetooth"));

                if (info == null)
                {
                    info = GetDefault();
                    InfoSave(info);
                }

                return info;
            }

            /// <summary>
            ///     Сохранение информации о COM устройствах в файловую систему.
            /// </summary>
            /// <param name="info">Сохраняемый экземпляр <see cref="BluetoothInfo" />.</param>
            private static void InfoSave(BluetoothInfo info)
            {
                JsonSerDer.SaveObject(info, PathManager.IoDevicePath("Bluetooth"));
            }

            /// <summary>
            ///     Фабричный метод для получения дефолтных параметров экземпляра класса.
            /// </summary>
            /// <returns>Экземпляр класса PortInfo с дефольными параметрами.</returns>
            private static BluetoothInfo GetDefault()
            {
                var newInfo = new BluetoothInfo
                {
                    NameDevice = "HCF97A02",
                    PasswordDevice = "hcaccess"
                };
                return newInfo;
            }

            #endregion
        }

        #endregion
    }
}