using HandControl.Model.Enums;

namespace HandControl.Model.BluetoothDto
{
    public class PackageDto
    {
        public CommandType Command { get; set; }
        public int PayloadSize { get; set; }
        public byte[] Payload { get; set; }
        public byte ReceivedCrc { get; set; }
        public byte Crc { get; set; }
    }
}