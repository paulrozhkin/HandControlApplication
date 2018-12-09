using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO.Ports;

namespace HandControl.Services
{
    class IODeviceCom: IIODevice
    {
        #region Variables
        public bool StateDeviceHand { get; private set; }
        public bool StateDeviceVoice { get; private set; }

        private SerialPort serialPortHand = new SerialPort();
        private SerialPort serialPortVoice = new SerialPort();
        private PortInfo infoCom;
        #endregion

        public IODeviceCom()
        {
            SerialSetup();
        }


        #region Methods
        bool IIODevice.SendToDevice(byte[] dataTx)
        {
            bool stateTx = false;

            if (dataTx[11] == CommunicationManager.addressHand)
            {
                if (StateDeviceHand == true)
                {
                    serialPortHand.Write(dataTx, 0, dataTx.Length);
                    stateTx = true;
                }
            }
            if (dataTx[11] == CommunicationManager.addressVoice)
            {
                if (StateDeviceVoice == true)
                {
                    serialPortVoice.Write(dataTx, 0, dataTx.Length);
                    stateTx = true;
                }
            }
            return stateTx;
        }

        byte[] IIODevice.ReceiveFromDevice(byte commandRx)
        {
            byte[] newData = null;

            if (StateDeviceHand == true)
            {
            }

            return newData;
        }

        private void SerialSetup()
        {
            bool stateConnectHand = false;
            bool stateConnectVoice = false;

            List<string> comPorts = new List<string>(SerialPort.GetPortNames());
            infoCom = PortInfo.InfoLoad();
            foreach (string realnamePort in comPorts)
            {
                if (realnamePort == infoCom.NamePortHand)
                {
                    serialPortHand.PortName = infoCom.NamePortHand;
                    serialPortHand.BaudRate = infoCom.BaudRateHand;
                    serialPortHand.Parity = Parity.None;
                    serialPortHand.StopBits = StopBits.One;
                    serialPortHand.DataBits = 8;
                    serialPortHand.Handshake = Handshake.None;
                    serialPortHand.RtsEnable = true;
                    
                    try
                    {
                        serialPortHand.Open();
                        serialPortHand.DiscardInBuffer();
                        serialPortHand.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandHandler);
                        stateConnectHand = true;
                    }
                    catch 
                    {
                        stateConnectHand = false;
                    }
                    
                }

                if (realnamePort == infoCom.NamePortVoice)
                {
                    serialPortVoice.PortName = infoCom.NamePortVoice;
                    serialPortVoice.BaudRate = infoCom.BaudRateVoice;
                    serialPortVoice.Parity = Parity.None;
                    serialPortVoice.StopBits = StopBits.One;
                    serialPortVoice.DataBits = 8;
                    serialPortVoice.Handshake = Handshake.None;
                    serialPortVoice.RtsEnable = true;

                    try
                    {
                        serialPortVoice.Open();
                        serialPortVoice.DiscardInBuffer();
                        serialPortVoice.DataReceived += new SerialDataReceivedEventHandler(DataReceivedVoiceHandler);
                        stateConnectVoice = true;
                    }
                    catch
                    {
                        stateConnectVoice = false;
                    }

                }

            }

            StateDeviceVoice = stateConnectVoice;
            StateDeviceHand = stateConnectHand;
        }

        private void DataReceivedHandHandler(object sender, SerialDataReceivedEventArgs e)
        {

        }

        private void DataReceivedVoiceHandler(object sender, SerialDataReceivedEventArgs e)
        {

        }

        #endregion

        private void Dispose()
        {
            if (serialPortHand.IsOpen)
            {
                serialPortHand.Close();
            }

            if (serialPortVoice.IsOpen)
            {
                serialPortVoice.Close();
            }
        }

    }

    class PortInfo
    {

        [JsonProperty(PropertyName = "PortHand")]
        public string NamePortHand { get; set; }
        [JsonProperty(PropertyName = "BaudRateHand")]
        public int BaudRateHand { get; set; }
        [JsonProperty(PropertyName = "PortVoice")]
        public string NamePortVoice { get; set; }
        [JsonProperty(PropertyName = "BaudRateVoice")]
        public int BaudRateVoice { get; set; }


        public static PortInfo GetDefault()
        {
            PortInfo newInfo = new PortInfo
            {
                BaudRateHand = 115200,
                NamePortHand = "None",
                BaudRateVoice = 115200,
                NamePortVoice = "None"
            };
            return newInfo;
        }


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

        public static void InfoSave(PortInfo info)
        {
            JsonSerDer.SaveObject(info, PathManager.IODevicePath("Com"));
        }
    }


}
