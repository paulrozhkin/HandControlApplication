﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandControl.Services;
using HandControl.Model;
using System.Collections.ObjectModel;

namespace HandControl.Services
{
    public static class CommunicationManager
    {
        #region Variables
        private static readonly IIODevice device = new IODeviceCom();

        private static readonly byte CommandSave = 0x15;
        private static readonly byte addressPC = 0x00;
        private static readonly byte addressHand = 0x01;
        private static readonly byte addressVoice = 0x02;

        private static readonly List<byte> versionProtocol = new List<byte>
        {
            Convert.ToByte('0'), Convert.ToByte('1')
        };

        private static readonly List<byte> startFiled = new List<byte> {
            Convert.ToByte('1'), Convert.ToByte('N'), Convert.ToByte('7'), Convert.ToByte('R'),
            Convert.ToByte('O'), Convert.ToByte('I'), Convert.ToByte('N'), Convert.ToByte('m')
        };

        private static readonly List<byte> endFiled = new List<byte> {
            Convert.ToByte('R'), Convert.ToByte('{'), Convert.ToByte('D'), Convert.ToByte('9'),
            Convert.ToByte('8'), Convert.ToByte('V'), Convert.ToByte('8'), Convert.ToByte('9')
        };
        #endregion

        #region Methods
        /// <summary>
        /// Создание и отправка пакета команды сохранения на устройство руки.
        /// </summary>
        /// <param name="commandsList"></param>
        public static void SaveCommands(ObservableCollection<CommandModel> commandsList)
        {
            if (device.StateDevice == true)
            {
                List<byte> dataField = new List<byte>
                {
                    CommandSave
                };
                foreach (CommandModel command in commandsList)
                {
                    dataField.AddRange(command.BinaryDate.ToList<byte>());
                }
                byte[] package = CreatePackage(addressHand, dataField);
                device.SendToDevice(package);
            }
        }

        public static void ExecuteTheCommand(string nameCommand = "")
        {
            if (device.StateDevice == true)
            {
                List<byte> dataField = new List<byte> { 2, 3 };
                byte[] package = CreatePackage(addressHand, dataField);
                device.SendToDevice(package);
            }
        }

        /// <summary>
        /// Создание байтового пакета отправляемого на устройство.
        /// </summary>
        /// <param name="distAddress"></param>
        /// <param name="dataField"></param>
        /// <returns></returns>
        private static byte[] CreatePackage(byte distAddress, List<byte> dataField)
        {
            // Стартовая константа
            List<byte> package = startFiled;

            // Заполнения поля информации пакета
            List<byte> infoField = versionProtocol;
            infoField.Add(addressPC);
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
            package.AddRange(endFiled);

            return package.ToArray<byte>();
        }
        #endregion
    }

    static class CRC8
    {
        static readonly byte[] CRC8_TABLE = new byte[]{
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

        public static byte Calculate(byte[] data, byte init = 0)
        {
            byte result = init;
            for (var i = 0; i < data.Length; i++)
            {
                result = CRC8_TABLE[result ^ data[i]];
            }
            return result;
        }
    }
}
