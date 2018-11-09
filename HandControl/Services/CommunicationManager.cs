using System;
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
        // Реализация CRC http://www.cyberforum.ru/csharp-beginners/thread1773550.html
        private static readonly IIODevice device = new IODeviceCom();

        private static readonly List<byte> versionProtocol = new List<byte>
        {
            Convert.ToByte(0), Convert.ToByte(1)
        };

        private static readonly List<byte> address = new List<byte>
        {
            Convert.ToByte(192), Convert.ToByte(168), Convert.ToByte(20), Convert.ToByte(0)
        };

        private static readonly List<byte> addressHand = new List<byte>
        {
            Convert.ToByte(192), Convert.ToByte(168), Convert.ToByte(20), Convert.ToByte(1)
        };

        private static readonly List<byte> addressVoice = new List<byte>
        {
            Convert.ToByte(192), Convert.ToByte(168), Convert.ToByte(20), Convert.ToByte(1)
        };

        private static readonly List<byte> startFiled = new List<byte> {
            Convert.ToByte('1'), Convert.ToByte('N'), Convert.ToByte('7'), Convert.ToByte('R'),
            Convert.ToByte('O'), Convert.ToByte('I'), Convert.ToByte('N'), Convert.ToByte('m')
        };

        private static readonly List<byte> endFiled = new List<byte> {
            Convert.ToByte('R'), Convert.ToByte('{'), Convert.ToByte('D'), Convert.ToByte('9'),
            Convert.ToByte('8'), Convert.ToByte('V'), Convert.ToByte('8'), Convert.ToByte('9')
        };


        public static void SaveCommands(ObservableCollection<CommandModel> commandsList)
        {
            if (device.StateDevice == false)
            {
                device.SendToDevice(new byte[] { Convert.ToByte(0x02) });
                List<byte> dataField = new List<byte>();
                foreach (CommandModel command in commandsList)
                {
                    dataField.AddRange(command.BinaryDate.ToList<byte>());
                }
                CreatePackage(addressHand ,dataField);
                // device.SendToDevice(package);
            }
        }

        private static byte[] CreatePackage(List<byte> distAddress,List<byte> dataField)
        {
            List<byte> package = startFiled;

            List<byte> infoField = versionProtocol;
            infoField.AddRange(address);
            infoField.AddRange(distAddress);
            

            List<byte> mainField = infoField; // info + data field
            mainField.AddRange(dataField);
            byte crc8 = CRC8.Calculate(package.ToArray<byte>());

            package.AddRange(mainField);
            package.Add(crc8);
            package.AddRange(endFiled);
            
            return package.ToArray<byte>();
        }
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
