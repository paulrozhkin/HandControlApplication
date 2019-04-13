// --------------------------------------------------------------------------------------
// <copyright file = "MainWindowViewModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;
    using HandControl.Model;
    using HandControl.Services;

    class MainWindowViewModel : ViewModelBase
    {
        #region Variables
        private GestureModel selectedCommand = null;
        public CommunicationManager Communication { get; set; }

        /// <summary>
        /// Список комманд
        /// </summary>
        public ObservableCollection<GestureModel> Commands { get; set; }

        ObservableCollection<GestureModel.MotionModel> commandMotions = new ObservableCollection<GestureModel.MotionModel>();
        public ObservableCollection<GestureModel.MotionModel> SelectedListCommandMotions
        {
            get
            {
                return commandMotions;
            }
            set
            {
                commandMotions = new ObservableCollection<GestureModel.MotionModel>();
                if (value != null)
                {
                    foreach (GestureModel.MotionModel action in value)
                    {
                        commandMotions.Add((GestureModel.MotionModel)action.Clone());
                    }
                }
                SelectedMotion = null;
            }
        }

        public GestureModel SelectedCommand
        {
            get
            {
                return selectedCommand;
            }
            set
            {
                if (value != null)
                {
                    selectedCommand = value.Clone() as GestureModel;
                    SelectedListCommandMotions = selectedCommand.ListMotions;
                }
                else
                {
                    SelectedListCommandMotions = null;
                    selectedCommand = null;
                }
            }
        }

        public GestureModel.MotionModel SelectedMotion { get; set; }

        public Visibility MotionVisible { get; set; } = Visibility.Collapsed;
        #endregion

        #region Commands
        public ICommand SaveMotionsCommand
        {
            get { return new RelayCommand((object obj) => this.SaveMotions()); }
        }

        public ICommand AddMotionCommand
        {
            get { return new RelayCommand((object obj) => this.AddMotion()); }
        }

        public ICommand DeleteCommandCommand
        {
            get { return new RelayCommand((object obj) => this.DeleteCommand(obj)); }
        }

        public ICommand DeleteMotionCommand
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
            // Commands = GestureModel.GetCommands();
            // Communication = CommunicationManager.GetInstance();


            // CommunicationManager.MotionListRequestCommand();
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
            GestureModel newCommand = GestureModel.GetDefault("Новая комманда");
            Commands.Add(newCommand);
        }

        private void DeleteACtion(object obj)
        {
            if (obj == null)
                return;

            int deletNameMotion = (int)obj;


            foreach (GestureModel.MotionModel actionModel in SelectedListCommandMotions)
            {
                if (actionModel.Id == Convert.ToInt32(deletNameMotion))
                {
                    for (int i = actionModel.Id; i < SelectedListCommandMotions.Count; i++)
                    {
                        SelectedListCommandMotions[i].Id = SelectedListCommandMotions[i].Id - 1;
                    }
                    SelectedListCommandMotions.Remove(actionModel);
                    break;
                }
            }
        }

        private void SaveMotions()
        {
            SelectedCommand.ListMotions = SelectedListCommandMotions;
            SelectedCommand.InfoGesture.Date = DateTime.Now.ToString("yyMMddHHmmss");
            GestureModel.Save(SelectedCommand);

            bool StateFountCommand = false;
            for (int i = 0; i < Commands.Count; i++)
            {
                if (Commands[i].Name == SelectedCommand.Name)
                {
                    Commands[i] = SelectedCommand.Clone() as GestureModel;
                    StateFountCommand = true;
                    break;
                    // SelectedCommand = Commands[i];
                }
            }

            if (StateFountCommand is false)
            {
                Commands.Add(SelectedCommand.Clone() as GestureModel);
            }

        }

        private void AddMotion()
        {
            GestureModel.MotionModel newMotion = GestureModel.MotionModel.GetDefault(GestureModel.MotionModel.GetNewId(SelectedListCommandMotions.ToList<GestureModel.MotionModel>()));
            SelectedListCommandMotions.Add(newMotion);
        }

        private void DeleteCommand(object obj)
        {
            if (obj == null)
                return;

            string deleteID = (string)obj;

            foreach (GestureModel command in Commands)
            {
                if (command.Name == deleteID)
                {
                    GestureModel.Delete(command);
                    Commands.Remove(command);
                    break;
                }
            }
        }

        private void HandHandler(object obj)
        {
            Communication.SaveCommands(Commands);
        }

        private void InfinityCheck(object obj)
        {
            SelectedCommand.InfoGesture.IterableMotions = !SelectedCommand.InfoGesture.IterableMotions;
        }

        private void CloseChange(object obj)
        {
            SelectedCommand = null;
            SelectedMotion = null;
        }
        #endregion
    }
}
