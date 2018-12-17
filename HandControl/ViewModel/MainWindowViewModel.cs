using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        ObservableCollection<ActionModel> commandActions = new ObservableCollection<ActionModel>();
        public ObservableCollection<ActionModel> CommandActions
        {
            get
            {
                return commandActions;
            }
            set
            {
                commandActions = new ObservableCollection<ActionModel>();
                foreach (ActionModel action in value)
                {
                    commandActions.Add((ActionModel)action.Clone());
                }
                SelectedAction = null;
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
                if (selectedCommand != null)
                {
                    CommandActions = selectedCommand.DataAction;
                }
                OnPropertyChanged();
            }
        }

        Model.ActionModel selectedAction;
        public ActionModel SelectedAction
        {
            get
            {
                return selectedAction;
            }
            set
            {
                if (value != null)
                {
                    selectedAction = (ActionModel)value;
                    ActionVisible = Visibility.Visible;
                }
                else
                {
                    ActionVisible = Visibility.Hidden;
                }
                OnPropertyChanged();
            }
        }

        private Visibility actionVisible;
        public Visibility ActionVisible
        {
            get
            {
                return actionVisible;
            }
            set
            {
                actionVisible = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        public ICommand SaveActionsCommand
        {
            get { return new RelayCommand((object obj) => this.SaveActions()); }
        }

        private void SaveActions()
        {
            SelectedCommand.DataAction = CommandActions;
            SelectedCommand.InfoCommand.Date = DateTime.Now.ToString("yyMMddHHmmss");
            CommandModel.SaveCommand(SelectedCommand);
            OnPropertyChanged();
        }

        public ICommand AddActionCommand
        {
            get { return new RelayCommand((object obj) => this.AddAction()); }
        }

        private void AddAction()
        {
            string newName = "Действие " + (CommandActions.Count + 1);
            ActionModel newAction = Model.ActionModel.GetDefault(newName);
            CommandActions.Add(newAction);
        }

        public ICommand DeleteItemCommand
        {
            get { return new RelayCommand((object obj) => this.DeleteItem()); }
        }

        private void DeleteItem()
        {
            CommandActions.Remove(SelectedAction);
            OnPropertyChanged();
        }
        #endregion

        public MainWindowViewModel()
        {
            Commands = CommandModel.GetCommands();
            if (Commands.Count() != 0)
                SelectedCommand = Commands[0];

            // CommunicationManager.SaveCommandsToVoice(Commands);
            // CommunicationManager.SaveCommands(commands);
            // CommunicationManager.ExecuteTheCommand("Сжать");
            // CommunicationManager.ExecuteTheCommand("ModeVoice");
            // CommunicationManager.ExecuteTheCommand(commands[0]);
            // CommunicationManager.ExecuteTheRaw(15000);
        }

    }
}
