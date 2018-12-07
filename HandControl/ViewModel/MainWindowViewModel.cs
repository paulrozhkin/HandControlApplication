using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HandControl.Model;
using HandControl.Services;

namespace HandControl.ViewModel
{
    class MainWindowViewModel
    {
        ObservableCollection<CommandModel> commands = CommandModel.GetCommands();
        public MainWindowViewModel()
        {
            // CommunicationManager.SaveCommands(commands);
            // CommunicationManager.ExecuteTheCommand("Сжать");
            // CommunicationManager.ExecuteTheCommand("ModeVoice");
            // CommunicationManager.ExecuteTheCommand(commands[0]);
            // CommunicationManager.ExecuteTheRaw(15000);
        }

    }
}
