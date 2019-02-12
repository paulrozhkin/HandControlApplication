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
        public event PropertyChangedEventHandler PropertyChanged;

        private CommandModel selectedCommand = null;
        public CommunicationManager Communication { get; set; }

        /// <summary>
        /// Список комманд
        /// </summary>
        public ObservableCollection<CommandModel> Commands { get; set; }

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
                if (value != null)
                {
                    selectedCommand = value.Clone() as CommandModel;
                    SelectedListCommandActions = selectedCommand.DataAction;
                }
                else
                {
                    SelectedListCommandActions = null;
                    selectedCommand = null;
                }
            }
        }

        public ActionModel SelectedAction { get; set; }

        public Visibility ActionVisible { get; set; } = Visibility.Collapsed;
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

        public ICommand AddCommandCommand
        {
            get { return new RelayCommand((object obj) => this.AddCommand(obj)); }
        }

        public ICommand HandCommand
        {
            get { return new RelayCommand((object obj) => this.HandHandler(obj)); }
        }

        public ICommand InfinityCheckCommand
        {
            get { return new RelayCommand((object obj) => this.InfinityCheck(obj)); }
        }

        public ICommand CloseChangeCommand
        {
            get { return new RelayCommand((object obj) => this.CloseChange(obj)); }
        }
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Commands = CommandModel.GetCommands();
            if (Commands.Count() != 0)
                SelectedCommand = Commands[0];

            Communication = CommunicationManager.GetInstance();
            // CommunicationManager.ActionListRequestCommand();
            // CommunicationManager.SaveCommandsToVoice(Commands);
            // CommunicationManager.SaveCommands(commands);
            // CommunicationManager.ExecuteTheCommand("Сжать");
            // CommunicationManager.ExecuteTheCommand("ModeVoice");
            // CommunicationManager.ExecuteTheCommand(commands[0]);
            // CommunicationManager.ExecuteTheRaw(15000);
        }
        #endregion

        #region Methods
        private void AddCommand(object obj)
        {
            CommandModel newCommand = new CommandModel
            {
                Name = "Новая комманда"
            };
            newCommand.InfoCommand = InfoCommandModel.GetDefault();
            newCommand.DataAction = new ObservableCollection<ActionModel>();
            Commands.Add(newCommand);
        }

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
                }

        private void SaveActions()
        {
            SelectedCommand.DataAction = SelectedListCommandActions;
            SelectedCommand.InfoCommand.Date = DateTime.Now.ToString("yyMMddHHmmss");
            CommandModel.SaveCommand(SelectedCommand);
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
                    CommandModel.DeleteCommand(command);
                    Commands.Remove(command);
                    break;
                }
            }
        }

        private void HandHandler(object obj)
        {
            throw new NotImplementedException();
        }

        private void InfinityCheck(object obj)
        {
            SelectedCommand.InfoCommand.IterableActions = !SelectedCommand.InfoCommand.IterableActions;
        }

        private void CloseChange(object obj)
        {
            SelectedCommand = null;
            SelectedAction = null;
        }
        #endregion
    }
}
