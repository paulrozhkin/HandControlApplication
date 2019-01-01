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
        #region Variables
        CommandModel selectedCommand = null;

        public event PropertyChangedEventHandler PropertyChanged;

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
        public ObservableCollection<ActionModel> SelectedListCommandActions
        {
            get
            {
                return commandActions;
            }
            set
            {
                commandActions = new ObservableCollection<ActionModel>();
                if (value != null)
                {
                    foreach (ActionModel action in value)
                    {
                        commandActions.Add((ActionModel)action.Clone());
                    }
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
                    SelectedListCommandActions = selectedCommand.DataAction;
                }
                OnPropertyChanged();
            }
        }

        ActionModel selectedAction;
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
        #endregion

        #region Commands
        public ICommand SaveActionsCommand
        {
            get { return new RelayCommand((object obj) => this.SaveActions()); }
        }

        

        public ICommand AddActionCommand
        {
            get { return new RelayCommand((object obj) => this.AddAction()); }
        }

       
        public ICommand DeleteCommandCommand
        {
            get { return new RelayCommand((object obj) => this.DeleteCommand(obj)); }
        }

        

        public ICommand DeleteActionCommand
        {
            get { return new RelayCommand((object obj) => this.DeleteACtion(obj)); }
        }

        
        #endregion

        #region Constructor
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
        #endregion

        #region Methods
        private void DeleteACtion(object obj)
                {
                    if (obj == null)
                        return;

                    int deletNameAction = (int)obj;
            

                    foreach(ActionModel actionModel in SelectedListCommandActions)
                    {
                        if (actionModel.Id == Convert.ToInt32(deletNameAction))
                        {
                            for (int i = actionModel.Id; i < SelectedListCommandActions.Count; i++)
                            {
                                SelectedListCommandActions[i].Id = SelectedListCommandActions[i].Id - 1;
                            }
                            SelectedListCommandActions.Remove(actionModel);
                            break;
                        }
                    }

                    OnPropertyChanged();
                }

        private void SaveActions()
        {
            SelectedCommand.DataAction = SelectedListCommandActions;
            SelectedCommand.InfoCommand.Date = DateTime.Now.ToString("yyMMddHHmmss");
            CommandModel.SaveCommand(SelectedCommand);
            OnPropertyChanged();
        }

        private void AddAction()
        {
            ActionModel newAction = ActionModel.GetDefault(ActionModel.GetNewId(SelectedListCommandActions.ToList<ActionModel>()));
            SelectedListCommandActions.Add(newAction);
        }

        private void DeleteCommand(object obj)
        {
            if (obj == null)
                return;

            string deletNameCommand = obj as string;

            foreach (CommandModel command in Commands)
            {
                if (command.Name == deletNameCommand)
                {
                    Commands.Remove(command);
                    break;
                }
            }

            OnPropertyChanged();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
