using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using HandControl.Model;
using InTheHand.Net;
using InTheHand.Net.Sockets;

namespace HandControl.Services.IODevice
{
    /// <summary>
    ///     Класса имплементирующий интерфейс IBluetoothService.
    ///     Выполняет непосредственную передачу через Bluetooth интерфейс.
    ///     \version 1.0
    ///     \date Апрель 2019 года
    ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public sealed class BluetoothService : IBluetoothService
    {
        /// <summary>
        ///     Буфер приема.
        /// </summary>
        private readonly byte[] _readBuffer = new byte[1024];

        private readonly Subject<byte[]> _receivedDataSubject = new Subject<byte[]>();

        /// <summary>
        ///     Ключ для соединения.
        /// </summary>
        private readonly Guid _serviceClassId;

        /// <summary>
        ///     Поток для данных bluetooth соединения.
        /// </summary>
        private volatile NetworkStream _bluetoothStream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BluetoothService" /> class.
        /// </summary>
        public BluetoothService()
        {
            // this guid is random, only need to match in Sender & Receiver  
            // this is like a "key" for the connection!  
            _serviceClassId = new Guid("00001101-0000-1000-8000-00805f9b34fb");
            ReceivedDataObservable = _receivedDataSubject.AsObservable();
        }

        public IObservable<byte[]> ReceivedDataObservable { get; }

        public void Dispose()
        {
            if (_bluetoothStream == null)
            {
                _bluetoothStream.Flush();
                _bluetoothStream.Close();
            }
        }

        /// <summary>
        ///     Gets only authenticated devices.
        /// </summary>
        /// <returns>The list of the devices.</returns>
        public async Task<IList<Device>> GetAuthenticDevices()
        {
            return await GetDevices(true);
        }

        /// <summary>
        ///     Gets all devices.
        /// </summary>
        /// <returns>The list of the devices.</returns>
        public async Task<IList<Device>> GetAllDevices()
        {
            return await GetDevices(false);
        }

        public async Task<bool> Connect(Device device)
        {
            var task = Task.Run(() =>
            {
                try
                {
                    if (_bluetoothStream != null) return false;

                    if (device == null) throw new ArgumentNullException(nameof(device));

                    var bluetoothClient = new BluetoothClient();
                    var ep = new BluetoothEndPoint(device.DeviceInfo.DeviceAddress, _serviceClassId);

                    // connecting  
                    bluetoothClient.Connect(ep);

                    // get stream for send the data  
                    _bluetoothStream = bluetoothClient.GetStream();

                    _bluetoothStream.BeginRead(_readBuffer, 0, _readBuffer.Length, BluetoothStreamReadHandler,
                        _bluetoothStream);

                    return true;
                }
                catch
                {
                    return false;
                }
            });

            return await task;
        }

        public void Disconnect()
        {
            if (_bluetoothStream != null)
            {
                _bluetoothStream.Flush();
                _bluetoothStream.Dispose();
                _bluetoothStream = null;
            }
        }

        /// <summary>
        ///     Sends the string UTF8 data to the Receiver.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="content">The content.</param>
        /// <returns>If was sent or not.</returns>
        public Task Send(byte[] content)
        {
            if (_bluetoothStream == null) throw new InvalidOperationException("Not connected");

            if (!content.Any()) throw new ArgumentNullException(nameof(content));

            Console.WriteLine("BYTES SENDED:{0}", content.Length);
            Console.WriteLine("DATA:{0}", content);

            return _bluetoothStream.WriteAsync(content, 0, content.Length);
        }

        private void BluetoothStreamReadHandler(IAsyncResult stream)
        {
            _bluetoothStream = (NetworkStream) stream.AsyncState;
            if (_bluetoothStream.CanRead)
            {
                var bytesRead = _bluetoothStream.EndRead(stream);
                var data = new byte[bytesRead];
                Array.Copy(_readBuffer, data, bytesRead);

                _bluetoothStream.BeginRead(_readBuffer, 0, _readBuffer.Length,
                    BluetoothStreamReadHandler, _bluetoothStream);

                _receivedDataSubject.OnNext(data);

                Console.WriteLine("BYTES RECEIVED:{0}", bytesRead);
                Console.WriteLine("DATA:{0}", data);
            }
        }

        /// <summary>
        ///     Gets bluetooth devices.
        /// </summary>
        /// <param name="isOnlyAuthenticDevices">Gets only authenticated devices?</param>
        /// <returns>The list of the devices.</returns>
        private async Task<IList<Device>> GetDevices(bool isOnlyAuthenticDevices)
        {
            // for not block the UI it will run in a different threat  
            var task = Task.Run(() =>
            {
                try
                {
                    var devices = new List<Device>();

                    using (var bluetoothClient = new BluetoothClient())
                    {
                        var array = bluetoothClient.DiscoverDevices(10, true, true, !isOnlyAuthenticDevices);
                        var count = array.Length;
                        for (var i = 0; i < count; i++) devices.Add(new Device(array[i]));
                    }

                    return devices;
                }
                catch
                {
                    return null;
                }
            });

            return await task;
        }
    }
}