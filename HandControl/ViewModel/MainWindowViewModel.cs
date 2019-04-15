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
        private GestureModel selectedGesture = null;
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

        public GestureModel SelectedGesture
        {
            get
            {
                return selectedGesture;
            }
            set
            {
                if (value != null)
                {
                    selectedGesture = value.Clone() as GestureModel;
                    SelectedListCommandMotions = selectedGesture.ListMotions;
                }
                else
                {
                    SelectedListCommandMotions = null;
                    selectedGesture = null;
                }
            }
        }

        public GestureModel.MotionModel SelectedMotion { get; set; }

        public Visibility MotionVisible { get; set; } = Visibility.Collapsed;
        #endregion

        #region Commands
        public ICommand SaveGestureCommand
        {
            get { return new RelayCommand((object obj) => this.SaveGesture()); }
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
            Commands = GestureModel.GetCommands();
            Communication = CommunicationManager.GetInstance();


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
            newCommand.ID = Guid.NewGuid();
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

        private void SaveGesture()
        {
            SelectedGesture.ListMotions = SelectedListCommandMotions;
            SelectedGesture.InfoGesture.Date = DateTime.Now.ToString("yyMMddHHmmss");
            GestureModel.Save(SelectedGesture);

            bool StateFountGesture = false;
            for (int i = 0; i < Commands.Count; i++)
            {
                if (Commands[i].ID == SelectedGesture.ID)
                {
                    Commands[i] = SelectedGesture.Clone() as GestureModel;
                    StateFountGesture = true;
                    break;
                }
            }

            if (StateFountGesture is false)
            {
                Commands.Add(SelectedGesture.Clone() as GestureModel);
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

            Guid deleteID = (Guid)obj;

            foreach (GestureModel command in Commands)
            {
                if (command.ID.Equals(deleteID))
                {
                    GestureModel.Delete(command);
                    Commands.Remove(command);
                    break;
                }
            }
        }

        private void HandHandler(object obj)
        {
            Communication.SaveGestures(Commands);
        }

        private void InfinityCheck(object obj)
        {
            SelectedGesture.InfoGesture.IterableGesture = !SelectedGesture.InfoGesture.IterableGesture;
        }

        private void CloseChange(object obj)
        {
            SelectedGesture = null;
            SelectedMotion = null;
        }
        #endregion
    }
}
