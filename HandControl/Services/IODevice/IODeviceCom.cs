// --------------------------------------------------------------------------------------
// <copyright file = "IODeviceCom.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO.Ports;
    using Newtonsoft.Json;

    /// <summary>
    /// Класса имплементирующий интерфейс IIODevice. 
    /// Выполняет ввод-вывод данных через COM порт.
    /// \brief Ввод-вывод данных через COM порт.
    /// \version 1.0
    /// \date Январь 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class IODeviceCom : IIODevice, IDisposable
    {
        #region Fields
        /// <summary>
        /// Ресурс COM порта для протеза руки.
        /// </summary>
        private readonly SerialPort serialPortHand = new SerialPort();

        /// <summary>
        /// Информация о конфигурации COM портов.
        /// </summary>
        private PortInfo infoCom;
        #endregion

        #region Constuctors
        /// <summary>
        /// Initializes a new instance of the <see cref="IODeviceCom" /> class. 
        /// Выполняет иницилизацию ресурсов COM порта согласно информации о конфигурации COM портов, 
        /// хранящихся в файле настроек.
        /// </summary>
        public IODeviceCom()
        {
            this.SerialSetup();
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
        /// Gets a value indicating whether состояние подключения протеза руки по COM порту.
        /// </summary>
        public bool StateDeviceHand { get; private set; }
        #endregion

        #region Methods
        /// <summary>
        /// Имплементация метода интерфейса IIODevice для передачи по COM порту.
        /// </summary>
        /// <param name="dataTx">Передаваемые данные.</param>
        /// <returns>Состояние отправки.</returns>
        void IIODevice.SendToDevice(byte[] dataTx)
        {
            try
            {
                this.serialPortHand.Write(dataTx, 0, dataTx.Length);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Имплементация метода интерфейса IIODevice для приема по COM порту.
        /// </summary>
        /// <param name="commandRx">Команда по которой устройство определяет возвращаемые данные.</param>
        /// <returns>Принятые данные, содержащие ответ на команду.</returns>
        byte[] IIODevice.ReceiveFromDevice(byte commandRx)
        {
            byte[] newData = null;

            if (this.StateDeviceHand == true)
            {
            }

            return newData;
        }

        /// <summary>
        /// Имплементация интерфейса IDisposable.
        /// Выполняет закрытие соединения COM порта.
        /// </summary>
        public void Dispose()
        {
            if (this.serialPortHand.IsOpen)
            {
                this.serialPortHand.Close();
            }
        }

        /// <summary>
        /// Метод для иницилизации подключения COM устройств на 
        /// основании информации хранящейся в экземпляре класса PortInfo.
        /// </summary>
        private void SerialSetup()
        {
            bool stateConnectHand = false;

            List<string> comPorts = new List<string>(SerialPort.GetPortNames());
            this.infoCom = PortInfo.InfoLoad();
            foreach (string realnamePort in comPorts)
            {
                if (realnamePort == this.infoCom.NamePortHand)
                {
                    this.serialPortHand.PortName = this.infoCom.NamePortHand;
                    this.serialPortHand.BaudRate = this.infoCom.BaudRateHand;
                    this.serialPortHand.Parity = Parity.None;
                    this.serialPortHand.StopBits = StopBits.One;
                    this.serialPortHand.DataBits = 8;
                    this.serialPortHand.Handshake = Handshake.None;
                    this.serialPortHand.RtsEnable = true;

                    try
                    {
                        this.serialPortHand.Open();
                        this.serialPortHand.DiscardInBuffer();
                        this.serialPortHand.DataReceived += new SerialDataReceivedEventHandler(this.DataReceivedHandHandler);
                        stateConnectHand = true;
                    }
                    catch
                    {
                        stateConnectHand = false;
                    }
                }
            }

            this.StateDeviceHand = stateConnectHand;
        }

        /// <summary>
        /// Обработчик принятие данных с устройства протеза.
        /// </summary>
        /// <param name="sender">Объект вызвавщий обработчик.</param>
        /// <param name="e">Событие вызвавщее обработчик.</param>
        private void DataReceivedHandHandler(object sender, SerialDataReceivedEventArgs e)
        {
        }

        public void ConnectDevice()
        {
            throw new NotImplementedException();
        }

        public void DisconnectDevice()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Classes
        /// <summary>
        /// Класса экземпляры которого содержат информацию о COM портах, к которым подключатся устройства.
        /// Выполняет загрузку и сохранение из файловой системы экземпляров. 
        /// \brief Информация о COM портах.
        /// \version 1.0
        /// \date Январь 2019 года
        /// \authors Paul Rozhkin(blackiiifox@gmail.com)
        /// </summary>
        public class PortInfo
        {
            #region Properties
            /// <summary>
            /// Gets or sets имя COM порта, 
            /// к которому подключен протез руки.
            /// </summary>
            [JsonProperty(PropertyName = "PortHand")]
            public string NamePortHand { get; set; }

            /// <summary>
            /// Gets or sets скорость передачи в бодах по COM порту,
            /// к которому подключен протез руки.
            /// </summary>
            [JsonProperty(PropertyName = "BaudRateHand")]
            public int BaudRateHand { get; set; }
            #endregion

            #region Methods
            /// <summary>
            /// Фабричный метод для получения дефолтных параметров экземпляра класса.
            /// </summary>
            /// <returns>Экземпляр класса PortInfo с дефольными параметрами.</returns>
            public static PortInfo GetDefault()
            {
                PortInfo newInfo = new PortInfo
                {
                    BaudRateHand = 115200,
                    NamePortHand = "None"
                };
                return newInfo;
            }

            /// <summary>
            /// Загрузка информации из файловой системы о параметрах конфигурации COM устройств.
            /// </summary>
            /// <returns>Экземпляр <see cref="PortInfo" />.</returns>
            public static PortInfo InfoLoad()
            {
                PortInfo info = (PortInfo)JsonSerDer.LoadObject<PortInfo>(PathManager.IODevicePath("Com"));

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
            /// <param name="info">Сохраняемый экземпляр <see cref="PortInfo" />.</param>
            public static void InfoSave(PortInfo info)
            {
                JsonSerDer.SaveObject(info, PathManager.IODevicePath("Com"));
            }
            #endregion
        }
        #endregion
    }
}
