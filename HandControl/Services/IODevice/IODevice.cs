using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO.Ports;

namespace HandControl.Services
{
    class IODeviceCom: IIODevice
    {
        #region Variables
        public bool StateDevice { get; private set; }

        private SerialPort serialPort = new SerialPort();
        private PortInfo infoCom;
        #endregion

        public IODeviceCom()
        {
            StateDevice = SerialSetup();
        }


        #region Methods
        bool IIODevice.SendToDevice(byte[] dataTx)
        {
            bool stateTx = false;
            if (StateDevice == true)
            {
                serialPort.Write(dataTx, 0, dataTx.Length);
                stateTx = true;
            }
            return stateTx;

        }

        byte[] IIODevice.ReceiveFromDevice(byte commandRx)
        {
            byte[] newData = null;

            if (StateDevice == true)
            {
            }

            return newData;
        }

        private bool SerialSetup()
        {
            bool stateConnect = false;
            List<string> comPorts = new List<string>(SerialPort.GetPortNames());
            infoCom = PortInfo.InfoLoad();
            foreach (string realnamePort in comPorts)
            {
                if (realnamePort == infoCom.NamePort)
                {
                    serialPort.PortName = infoCom.NamePort;
                    serialPort.BaudRate = infoCom.BaudRate;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    serialPort.DataBits = 8;
                    serialPort.Handshake = Handshake.None;
                    serialPort.RtsEnable = true;
                    
                    try
                    {
                        serialPort.Open();
                        serialPort.DiscardInBuffer();
                        serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        stateConnect = true;
                    }
                    catch 
                    {
                        stateConnect = false;
                    }
                    
                }
            }

            return stateConnect;
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

        }


            #endregion

        private void Dispose()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

    }

    class PortInfo
    {

        [JsonProperty(PropertyName = "Port")]
        public string NamePort { get; set; }
        [JsonProperty(PropertyName = "BaudRate")]
        public int BaudRate { get; set; }

        public static PortInfo GetDefault()
        {
            PortInfo newInfo = new PortInfo();
            newInfo.BaudRate = 115200;
            newInfo.NamePort = "None";
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
