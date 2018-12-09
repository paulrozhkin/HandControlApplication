using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HandControl.Model;
using HandControl.Services;

namespace HandControl.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        CommandModel selectedCommand = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        ObservableCollection<CommandModel> commands;
        public ObservableCollection<CommandModel> Commands {
            get
            {
                return commands;
            }
            set
            {
                commands = value;
                OnPropertyChanged();
            }
        }

        public CommandModel SelectedCommand
        {
            get
            {
                return selectedCommand;
            }
            set
            {
                selectedCommand = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            Commands = CommandModel.GetCommands();
            CommunicationManager.SaveCommandsToVoice(Commands);
            // CommunicationManager.SaveCommands(commands);
            // CommunicationManager.ExecuteTheCommand("Сжать");
            // CommunicationManager.ExecuteTheCommand("ModeVoice");
            // CommunicationManager.ExecuteTheCommand(commands[0]);
            // CommunicationManager.ExecuteTheRaw(15000);
        }

    }
}
