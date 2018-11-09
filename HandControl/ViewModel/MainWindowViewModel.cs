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
        ObservableCollection<CommandModel> commands;
        public MainWindowViewModel()
        {
            commands = CommandModel.GetCommands();
            CommunicationManager.SaveCommands(commands);
        }
    }
}
