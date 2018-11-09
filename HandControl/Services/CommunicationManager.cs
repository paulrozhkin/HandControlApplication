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
        private static readonly IIODevice device = new IODeviceCom();

        public static void SaveCommands(ObservableCollection<CommandModel> commandsList)
        {
            if (device.StateDevice == true)
            {
                // device.SendToDevice(new byte[] { Convert.ToByte(0x02) });
                // foreach (CommandModel command in commandsList)
                //    device.SendToDevice(command.BinaryDate);
            }
        }
    }
}
