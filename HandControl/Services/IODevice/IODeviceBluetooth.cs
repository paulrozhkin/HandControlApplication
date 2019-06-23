// --------------------------------------------------------------------------------------
// <copyright file = "IODeviceBluetooth.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Bluetooth.Model;
    using Bluetooth.Services;
    using GalaSoft.MvvmLight;
    using InTheHand.Net;
    using InTheHand.Net.Bluetooth;
    using InTheHand.Net.Sockets;
    using Newtonsoft.Json;

    /// <summary>
    /// Класса имплементирующий интерфейс IIODevice. 
    /// Выполняет ввод-вывод данных через Bluetooth.
    /// \brief Ввод-вывод данных через Bluetooth порт.
    /// \version 1.0
    /// \date Апрель 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class IODeviceBluetooth : IIODevice, IDisposable
    {
        #region Constructors
        private readonly SenderBluetoothService senderService;
        private readonly ReceiverBluetoothService receiverService;
        BluetoothInfo bluetoothInfo;
        Device device;

        /// <summary>
        /// Initializes a new instance of the <see cref="IODeviceBluetooth" /> class.
        /// </summary>
        public IODeviceBluetooth()
        {
            senderService = new SenderBluetoothService();
            receiverService = new ReceiverBluetoothService();
            bluetoothInfo = BluetoothInfo.InfoLoad();
            // receiverService.Start(null);
        }
        #endregion

        #region Events
        /// <summary>
        /// Имплементация INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        /// <summary>
        /// Gets a value indicating whether состояние подключения протеза руки по Bluetooth.
        /// </summary>
        public bool StateDeviceHand { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Имплементация интерфейса IDisposable.
        /// Выполняет закрытие соединения Bluetooth.
        /// </summary>
        public void Dispose()
        {
            senderService.Dispose();
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Имплементация метода интерфейса IIODevice для приема по Bluetooth.
        /// </summary>
        /// <param name="commandRx">Команда по которой устройство определяет возвращаемые данные.</param>
        /// <returns>Принятые данные, содержащие ответ на команду.</returns>
        public byte[] ReceiveFromDevice(byte commandRx)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Имплементация метода интерфейса IIODevice для передачи по Bluetooth.
        /// </summary>
        /// <param name="dataTx">Передаваемые данные.</param>
        /// <returns>Состояние отправки.</returns>
        public void SendToDevice(byte[] dataTx)
        {
            _ = senderService.Send(device, dataTx);
            //try
            //{
            //    Task task = senderService.Send(device, dataTx);
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
            // throw new NotImplementedException();
        }

        private async Task ConnectToDeviceAsync()
        {
            while (!this.StateDeviceHand)
            {
                if (BluetoothRadio.IsSupported.Equals(true))
                {
                    var listDevices = await senderService.GetAuthenticDevices();

                    foreach (Device device in listDevices)
                    {
                        if (device.DeviceName.Equals(bluetoothInfo.NameDevice))
                        {
                            if (!device.DeviceInfo.Authenticated)
                            {
                                string password = "hcaccess";
                                if (!BluetoothSecurity.PairRequest(device.DeviceInfo.DeviceAddress, password))
                                {
                                    throw new IOException("Bluetooth pair error. Name: " + device.DeviceName + ", pass: " + password);
                                }
                            }

                            this.device = device;

                            bool status = await senderService.Connect(device);


                            if (status == true)
                            {
                                this.StateDeviceHand = true;
                                Thread threadCheck = new Thread(new ThreadStart(ConnectionCheackerAsync));
                                threadCheck.Start();
                            }

                            break;
                        }
                    }
                }

                await Task.Delay(1000);
            }

        }

        public async void ConnectionCheackerAsync()
        {
            while (true)
            {
                var listDevices = await senderService.GetAuthenticDevices();

                foreach (Device device in listDevices)
                {
                    if (device.DeviceName.Equals(bluetoothInfo.NameDevice))
                    {
                        if (!device.IsConnected)
                        {
                            senderService.Disconnect();
                            this.StateDeviceHand = false;
                            //_= ConnectToDeviceAsync();
                            Thread.CurrentThread.Interrupt();
                        }
                    }
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


        public void ReceiveFromDevice(string asd)
        {
            throw new NotImplementedException();
        }

        private void CheckConnect()
        {
            while (true)
            {
                // senderService.Send();
                Thread.Sleep(5000);
            }
        }

        public void ConnectDevice()
        {
            // Thread thread = new Thread(CheckConnect);
            // thread.Start();
            _ = ConnectToDeviceAsync();

        }

        public void DisconnectDevice()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Classes
        /// <summary>
        /// Класса имплементирующий интерфейс ISenderBluetoothService. 
        /// Выполняет непосредственную передачу через Bluetooth интерфейс.
        /// \version 1.0
        /// \date Апрель 2019 года
        /// \authors Paul Rozhkin(blackiiifox@gmail.com)
        /// </summary>
        public sealed class SenderBluetoothService : ISenderBluetoothService, IDisposable
        {
            /// <summary>
            /// Ключ для соединения.
            /// </summary>
            private readonly Guid serviceClassId;

            /// <summary>
            /// Поток для данных bluetooth соединения.
            /// </summary>
            private volatile System.Net.Sockets.NetworkStream bluetoothStream;

            /////// <summary>
            /////// Поток для чтения данных bluetooth.
            /////// </summary>
            ////BinaryReader readerBluetooth;

            /////// <summary>
            /////// Поток для записи данных bluetooth.
            /////// </summary>
            ////BinaryWriter writerBluetooth;

            /// <summary>
            /// Буфер приема.
            /// </summary>
            byte[] readBuffer = new byte[1024];

            /// <summary>  
            /// Initializes a new instance of the <see cref="SenderBluetoothService"/> class.   
            /// </summary>  
            public SenderBluetoothService()
            {
                // this guid is random, only need to match in Sender & Receiver  
                // this is like a "key" for the connection!  
                this.serviceClassId = new Guid("00001101-0000-1000-8000-00805f9b34fb");
            }

            public void Dispose()
            {
                if (bluetoothStream == null)
                {
                    bluetoothStream.Flush();
                    bluetoothStream.Close();
                }
            }

            /// <summary>
            /// Gets bluetooth devices.
            /// </summary>
            /// <param name="isOnlyAuthenticDevices">Gets only authenticated devices?</param>
            /// <returns>The list of the devices.</returns>
            public async Task<IList<Device>> GetDevices(bool isOnlyAuthenticDevices)
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
                            for (var i = 0; i < count; i++)
                            {
                                devices.Add(new Device(array[i]));
                            }
                        }

                        return devices;
                    }
                    catch
                    {
                        return null;
                        // throw new IOException("Blueooth client error.");
                    }
                });

                return await task;
            }


            /// <summary>  
            /// Gets only authenticated devices.
            /// </summary>  
            /// <returns>The list of the devices.</returns>  
            public async Task<IList<Device>> GetAuthenticDevices()
            {
                return await GetDevices(true);
            }

            /// <summary>
            /// Gets all devices.  
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
                        if (bluetoothStream == null)
                        {
                            if (device == null)
                            {
                                throw new ArgumentNullException("device");
                            }

                            var bluetoothClient = new BluetoothClient();
                            var ep = new BluetoothEndPoint(device.DeviceInfo.DeviceAddress, serviceClassId);

                            // connecting  
                            bluetoothClient.Connect(ep);

                            // get stream for send the data  
                            bluetoothStream = bluetoothClient.GetStream();
                            ////readerBluetooth = new BinaryReader(bluetoothStream);
                            ////writerBluetooth = new BinaryWriter(bluetoothStream);

                            bluetoothStream.BeginRead(readBuffer, 0, readBuffer.Length, BluetoothStreamReadHandler, bluetoothStream);

                            //readerBluetooth.BaseStream.
                        }

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
                if (bluetoothStream != null)
                {
                    bluetoothStream.Flush();
                    bluetoothStream.Dispose();
                }
            }

            void BluetoothStreamReadHandler(IAsyncResult stream)
            {
                bluetoothStream = (NetworkStream)stream.AsyncState;
                if (bluetoothStream.CanRead)
                {
                    byte[] readBuffer = new byte[1024];
                    int bytesRead;

                    bytesRead = bluetoothStream.EndRead(stream);
                    byte[] data = new byte[bytesRead];
                    Array.Copy(readBuffer, data, bytesRead);
                    bluetoothStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(BluetoothStreamReadHandler), bluetoothStream);

                    Console.WriteLine("BYTES RECEIVED:{0}", bytesRead);
                    Console.WriteLine("DATA:{0}", data);
                }

            }

            /// <summary>  
            /// Sends the string UTF8 data to the Receiver.  
            /// </summary>  
            /// <param name="device">The device.</param>  
            /// <param name="content">The content.</param>  
            /// <returns>If was sent or not.</returns>  
            public async Task<bool> Send(Device device, byte[] content)
            {
                var task = Task.Run(() =>
                {
                    if (content.Count() == 0 && content.Any())
                    {
                        throw new ArgumentNullException("content");
                    }

                    //while (!bluetoothStream.DataAvailable) ;
                    //byte[] buffer = new byte[1000];
                    //this.bluetoothStream.Read(buffer, 0 ,1000);
                    lock (bluetoothStream)
                    {
                        bluetoothStream.Write(content, 0, content.Length);
                    }

                    Console.WriteLine("BYTES SENDED:{0}", content.Length);
                    Console.WriteLine("DATA:{0}", content);
                    // bluetoothStream.Write(content, 0, content.Length);

                    return true;
                });

                return await task;
            }

            /// <summary>  
            /// Sends the binary data to the Receiver.  
            /// </summary>  
            /// <param name="device">The device.</param>  
            /// <param name="content">The content.</param>  
            /// <returns>If was sent or not.</returns>  
            public async Task<bool> Send(Device device, string content)
            {
                if (device == null)
                {
                    throw new ArgumentNullException("Device empty.");
                }


                if (string.IsNullOrEmpty(content))
                {
                    throw new ArgumentNullException("content");
                }

                // for not block the UI it will run in a different threat  
                var task = Task.Run(() =>
                    {
                        using (var bluetoothClient = new BluetoothClient())
                        {
                            try
                            {
                                var ep = new BluetoothEndPoint(device.DeviceInfo.DeviceAddress, serviceClassId);

                                // connecting  
                                bluetoothClient.Connect(ep);

                                // get stream for send the data  
                                var bluetoothStream = bluetoothClient.GetStream();

                                // if all is ok to send  
                                if (bluetoothClient.Connected && bluetoothStream != null)
                                {
                                    // write the data in the stream
                                    var buffer = System.Text.Encoding.UTF8.GetBytes(content);
                                    bluetoothStream.Write(buffer, 0, buffer.Length);
                                    bluetoothStream.Flush();
                                    bluetoothStream.Close();
                                    return true;
                                }

                                return false;
                            }
                            catch
                            {
                                // the error will be ignored and the send data will report as not sent  
                                // for understood the type of the error, handle the exception
                            }
                        }

                        return false;
                    });

                return await task;
            }
        }

        /// <summary>
        /// Класса экземпляры которого содержат информацию о Bluetooth устройства.
        /// Выполняет загрузку и сохранение из файловой системы экземпляров. 
        /// \brief Информация о Bluetooth устройстве.
        /// \authors Paul Rozhkin(blackiiifox@gmail.com)
        /// </summary>
        private class BluetoothInfo
        {
            #region Properties
            /// <summary>
            /// Gets or sets имя Bluetooth устройства протеза руки.
            /// </summary>
            [JsonProperty(PropertyName = "Name")]
            public string NameDevice { get; set; }

            /// <summary>
            /// Gets or sets пароль Bluetooth устройства протеза руки.
            /// </summary>
            [JsonProperty(PropertyName = "Password")]
            public string PasswordDevice { get; set; }
            #endregion

            #region Methods
            /// <summary>
            /// Фабричный метод для получения дефолтных параметров экземпляра класса.
            /// </summary>
            /// <returns>Экземпляр класса PortInfo с дефольными параметрами.</returns>
            public static BluetoothInfo GetDefault()
            {
                BluetoothInfo newInfo = new BluetoothInfo
                {
                    NameDevice = "HCF97A02",
                    PasswordDevice = "hcaccess"
                };
                return newInfo;
            }

            /// <summary>
            /// Загрузка информации из файловой системы о параметрах конфигурации COM устройств.
            /// </summary>
            /// <returns>Экземпляр <see cref="BluetoothInfo" />.</returns>
            public static BluetoothInfo InfoLoad()
            {
                BluetoothInfo info = (BluetoothInfo)JsonSerDer.LoadObject<BluetoothInfo>(PathManager.IODevicePath("Bluetooth"));

                if (info == null)
                {
                    info = GetDefault();
                    InfoSave(info);
                }

                return info;
            }

            /// <summary>
            /// Сохранение информации о COM устройствах в файловую систему.
            /// </summary>
            /// <param name="info">Сохраняемый экземпляр <see cref="BluetoothInfo" />.</param>
            public static void InfoSave(BluetoothInfo info)
            {
                JsonSerDer.SaveObject(info, PathManager.IODevicePath("Bluetooth"));
            }
            #endregion
        }
        #endregion
    }
}
