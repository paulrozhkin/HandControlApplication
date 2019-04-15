// --------------------------------------------------------------------------------------
// <copyright file = "CommunicationManager.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using HandControl.Model;

    /// <summary>
    /// Класс, предоставляющий API для управления устройствами системы.
    /// Является имплементацией паттерна Singleton.
    /// \version 1.0
    /// \date Январь 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class CommunicationManager : INotifyPropertyChanged
    {
        #region Fields
        /// <summary>
        /// Текущая версия протокола.
        /// </summary>
        private const string VersionProtocol = "01";

        /// <summary>
        /// Стартовое поле в формате протокола передачи.
        /// </summary>
        private const string StartFiled = "1N7ROINm";

        /// <summary>
        /// Стоповое поле в формате протокола передачи.
        /// </summary>
        private const string StopFiled = "R{D98V89";

        /// <summary>
        /// Обеспечивает потокобезопасное извлечение instance.  
        /// </summary>
        private static readonly object SyncRoot = new object();

        /// <summary>
        /// Хранит одиночный экземпляр класса <see cref="CommunicationManager"/>.
        /// </summary>
        private static CommunicationManager instance;
        #endregion

        #region Constuctors
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunicationManager" /> class.
        /// </summary>
        public CommunicationManager()
        {
            this.СonnectedDevices = new IODeviceCom();
        }
        #endregion

        #region Events
        /// <summary>
        /// Имплементация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Enums
        /// <summary>
        /// Определяет режимы управления протеза.
        /// </summary>
        public enum OperationModeType
        {
            /// <summary>
            /// Миоэлектрический режим работы. В этом режиме прямое управление отключен.
            /// Моторно-двигательные функции протеза реализуются за счет считывание биопотенциалов с мышц.
            /// </summary>
            [Description("Миоэлектрический режим работы")] MyoelectricMode,

            /// <summary>
            /// Режим прямого управления. В этом режиме миоэлектрическое управление отключено.
            /// Моторно-двигательные функции протеза реализуются за счет внешних устройств, подключаемых к протезу,
            /// в частности мобильный телефон.
            /// </summary>
            [Description("Режим прямого управления")] DirectMode,

            /// <summary>
            /// Комбинированный режим управления. В этом режиме используются и миоэлектрическое управление и прямое управление.
            /// </summary>
            [Description("Комбинированный режим управления")] DirectModeCombinedMode
        }

        /// <summary>
        /// Определяет команды, реализуемые протезом.
        /// </summary>
        private enum CommandType
        {
            /// <summary>
            /// Команда сохранения одного или нескольких жестов протеза.
            /// </summary>
            [Description("Сохранить жест")] Save = 0x15,

            /// <summary>
            /// Команда выполнения жеста протезом по имени.
            /// </summary>
            [Description("Выполнить жест по имени")] ExecByName = 0x16,

            /// <summary>
            /// Команда выполнения протезом руки переданного жеста.
            /// </summary>
            [Description("Выполнить переданный жест")] ExecByMotion = 0x17,

            /// <summary>
            /// Команда установки переданных положений пальцев на протез.
            /// </summary>
            [Description("Установить на все двигатели переданное значение")] ExexRaw = 0x18,
        }

        /// <summary>
        /// Определяет устройства системы.
        /// </summary>
        private enum DeviceType
        {
            /// <summary>
            /// Персональный комьютер.
            /// </summary>
            [Description("Персональный комьютер")] PC = 0x00,

            /// <summary>
            /// Устройство протеза.
            /// </summary>
            [Description("Протез")] Prosthesis = 0x01,

            /// <summary>
            /// Внешнее устройство управления.
            /// </summary>
            [Description("Внешнее устройство управления")] Сontrol = 0x02,
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets экземпляр, выполняющий соедниение с устройством протеза.
        /// </summary>
        private IIODevice СonnectedDevices { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// Получить единичный экземпляр класса <see cref="CommunicationManager"/>.
        /// </summary>
        /// <returns>Единичный экземпляр класса <see cref="CommunicationManager"/>.</returns>
        public static CommunicationManager GetInstance()
        {
            if (instance == null)
            {
                lock (SyncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CommunicationManager();
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// Создание и отправка пакета жестов на устройство протеза для
        /// сохранения их на устройстве и будующего применения.
        /// </summary>
        /// <param name="gestureList">Коллекция передаваемых жестов.</param>
        public void SaveGestures(ObservableCollection<GestureModel> gestureList)
        {
            List<byte> dataField = new List<byte>
                {
                    (byte)CommandType.Save
                };

            foreach (GestureModel command in gestureList)
            {
                dataField.AddRange(command.BinaryDate.ToList<byte>());
            }

            byte[] package = CreatePackage((byte)DeviceType.Prosthesis, dataField);

            this.СonnectedDevices.SendToDevice(package);
        }

        /// <summary>
        /// Создание и отправка пакета на устройство протеза для установки требуемого положения на все сервоприводы.
        /// </summary>
        /// <param name="newValueServo">Устанавливаемое положение</param>
        public void ExecuteTheRaw(uint newValueServo)
        {
            List<byte> dataField = new List<byte>
                {
                    (byte)CommandType.ExexRaw
                };
            List<byte> valueByte = BitConverter.GetBytes(newValueServo).ToList<byte>();
            dataField.AddRange(valueByte);
            byte[] package = CreatePackage((byte)DeviceType.Prosthesis, dataField);
            this.СonnectedDevices.SendToDevice(package);
        }

        /// <summary>
        /// Создание и отправка пакета на устройство протеза для исполнения переданного жеста.
        /// </summary>
        /// <param name="gesture">Исполняемый жест.</param>
        public void ExecuteTheGesture(GestureModel gesture)
        {
            List<byte> dataField = new List<byte> { (byte)CommandType.ExecByMotion };
            dataField.AddRange(gesture.BinaryDate.ToList<byte>());
            byte[] package = CreatePackage((byte)DeviceType.Prosthesis, dataField);
            this.СonnectedDevices.SendToDevice(package);
        }

        /// <summary>
        /// Создание и отправка пакета на устройство протеза для исполнения заложенного жеста по имени.
        /// </summary>
        /// <param name="nameGesture">Имя заложенного жеста.</param>
        public void ExecuteTheGesture(string nameGesture)
        {
            List<byte> dataField = new List<byte> { (byte)CommandType.ExecByName };

            byte[] byteName = Encoding.UTF8.GetBytes(nameGesture);

            if (byteName.Length == 20)
            {
                byteName[18] = Convert.ToByte('\0');
                byteName[19] = Convert.ToByte('\0');
            }

            List<byte> name = byteName.ToList<byte>();

            for (int i = byteName.Count(); i < 20; i++)
            {
                name.Add(Convert.ToByte('\0'));
            }

            dataField.AddRange(name);

            byte[] package = CreatePackage((byte)DeviceType.Prosthesis, dataField);
            this.СonnectedDevices.SendToDevice(package);
        }

        /// <summary>
        /// Создание байтового пакета отправляемого на устройство.
        /// </summary>
        /// <param name="distAddress">Адрес устройства назначения.</param>
        /// <param name="dataField">Передаваемые данные.</param>
        /// <returns>Сформированный байтовый пакет.</returns>
        private static byte[] CreatePackage(byte distAddress, List<byte> dataField)
        {
            // Стартовая константа
            List<byte> package = new List<byte>();
            package.AddRange(Encoding.ASCII.GetBytes(StartFiled));

            // Заполнения поля информации пакета
            List<byte> infoField = new List<byte>();
            infoField.AddRange(Encoding.ASCII.GetBytes(VersionProtocol));
            infoField.Add((byte)DeviceType.Prosthesis);
            infoField.Add(distAddress);
            uint countField = Convert.ToUInt32(dataField.Count());

            // Заполнение поля размера информационного пакета
            byte[] countFieldByte = new byte[4];
            countFieldByte[3] = Convert.ToByte(countField & 0x000000FF);
            countFieldByte[2] = Convert.ToByte((countField & 0x0000FF00) >> 8);
            countFieldByte[1] = Convert.ToByte((countField & 0x00FF0000) >> 16);
            countFieldByte[0] = Convert.ToByte((countField & 0xFF000000) >> 24);
            infoField.AddRange(countFieldByte.ToList<byte>());

            // Создание основного контейнера данных(без CRC,стартовых и стоповых констант)
            List<byte> mainField = infoField; // info + data field
            byte crc8 = CRC8.Calculate(dataField.ToArray<byte>());
            mainField.AddRange(dataField);
            mainField.Add(crc8);

            // Добавление в конечный пакет основного контейнера, crc кода и стоповой последовательности
            package.AddRange(mainField);
            package.AddRange(Encoding.ASCII.GetBytes(StopFiled));

            return package.ToArray<byte>();
        }
        #endregion

        #region Classes
        /// <summary>
        /// Класс для расчета циклического избыточного кода для байт (CRC8).
        /// \authors Paul Rozhkin(blackiiifox@gmail.com)
        /// </summary>
        private static class CRC8
        {
            /// <summary>
            /// Таблица CRC8.
            /// </summary>
            private static readonly byte[] TableCRC8 = new byte[]
            {
                0, 94, 188, 226, 97, 63, 221, 131, 194, 156, 126, 32, 163, 253, 31, 65,
                157, 195, 33, 127, 252, 162, 64, 30, 95, 1, 227, 189, 62, 96, 130, 220,
                35, 125, 159, 193, 66, 28, 254, 160, 225, 191, 93, 3, 128, 222, 60, 98,
                190, 224, 2, 92, 223, 129, 99, 61, 124, 34, 192, 158, 29, 67, 161, 255,
                70, 24, 250, 164, 39, 121, 155, 197, 132, 218, 56, 102, 229, 187, 89, 7,
                219, 133, 103, 57, 186, 228, 6, 88, 25, 71, 165, 251, 120, 38, 196, 154,
                101, 59, 217, 135, 4, 90, 184, 230, 167, 249, 27, 69, 198, 152, 122, 36,
                248, 166, 68, 26, 153, 199, 37, 123, 58, 100, 134, 216, 91, 5, 231, 185,
                140, 210, 48, 110, 237, 179, 81, 15, 78, 16, 242, 172, 47, 113, 147, 205,
                17, 79, 173, 243, 112, 46, 204, 146, 211, 141, 111, 49, 178, 236, 14, 80,
                175, 241, 19, 77, 206, 144, 114, 44, 109, 51, 209, 143, 12, 82, 176, 238,
                50, 108, 142, 208, 83, 13, 239, 177, 240, 174, 76, 18, 145, 207, 45, 115,
                202, 148, 118, 40, 171, 245, 23, 73, 8, 86, 180, 234, 105, 55, 213, 139,
                87, 9, 235, 181, 54, 104, 138, 212, 149, 203, 41, 119, 244, 170, 72, 22,
                233, 183, 85, 11, 136, 214, 52, 106, 43, 117, 151, 201, 74, 20, 246, 168,
                116, 42, 200, 150, 21, 75, 169, 247, 182, 232, 10, 84, 215, 137, 107, 53
            };

            /// <summary>
            /// Расчет CRC8.
            /// </summary>
            /// <param name="data">Байтовые данные.</param>
            /// <returns>CRC8 код.</returns>
            public static byte Calculate(byte[] data)
            {
                byte result = 0;
                for (var i = 0; i < data.Length; i++)
                {
                    result = TableCRC8[result ^ data[i]];
                }

                return result;
            }
        }
        #endregion
    }
}
