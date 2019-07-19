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
        /// Список жестов протеза.
        /// </summary>
        public ObservableCollection<GestureModel> ListGesture { get; set; }

        ObservableCollection<GestureModel.MotionModel> ListMotions = new ObservableCollection<GestureModel.MotionModel>();
        public ObservableCollection<GestureModel.MotionModel> SelectedListGestureMotions
        {
            get
            {
                return ListMotions;
            }
            set
            {
                ListMotions = new ObservableCollection<GestureModel.MotionModel>();
                if (value != null)
                {
                    foreach (GestureModel.MotionModel action in value)
                    {
                        ListMotions.Add((GestureModel.MotionModel)action.Clone());
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
                    SelectedListGestureMotions = new ObservableCollection<GestureModel.MotionModel>(selectedGesture.ListMotions);
                }
                else
                {
                    SelectedListGestureMotions = null;
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

        public ICommand DeleteGestureCommand
        {
            get { return new RelayCommand((object obj) => this.DeleteGesture(obj)); }
        }

        public ICommand DeleteMotionCommand
        {
            get { return new RelayCommand((object obj) => this.DeleteMotion(obj)); }
        }

        public ICommand AddGestureCommand
        {
            get { return new RelayCommand((object obj) => this.AddGesture(obj)); }
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
            ListGesture = GestureModel.GetGestures();
            Communication = CommunicationManager.GetInstance();
            Communication.СonnectedDevices.ConnectDevice();

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
        private void AddGesture(object obj)
        {
            GestureModel newGesture = GestureModel.GetDefault(Guid.NewGuid(), "Новый жест");
            ListGesture.Add(newGesture);
        }

        private void DeleteMotion(object obj)
        {
            if (obj == null)
                return;

            int deletNameMotion = (int)obj;


            foreach (GestureModel.MotionModel actionModel in SelectedListGestureMotions)
            {
                if (actionModel.Id == Convert.ToInt32(deletNameMotion))
                {
                    for (int i = actionModel.Id; i < SelectedListGestureMotions.Count; i++)
                    {
                        SelectedListGestureMotions[i].Id = SelectedListGestureMotions[i].Id - 1;
                    }
                    SelectedListGestureMotions.Remove(actionModel);
                    break;
                }
            }
        }

        private void SaveGesture()
        {
            SelectedGesture.ListMotions = SelectedListGestureMotions.ToList();
            SelectedGesture.InfoGesture.TimeChange = DateTime.Now;
            SelectedGesture.InfoGesture.NumberOfMotions = SelectedListGestureMotions.Count();
            GestureModel.Save(SelectedGesture);

            bool StateFountGesture = false;
            for (int i = 0; i < ListGesture.Count; i++)
            {
                if (ListGesture[i].ID == SelectedGesture.ID)
                {
                    ListGesture[i] = SelectedGesture.Clone() as GestureModel;
                    StateFountGesture = true;
                    break;
                }
            }

            if (StateFountGesture is false)
            {
                ListGesture.Add(SelectedGesture.Clone() as GestureModel);
            }

        }

        private void AddMotion()
        {
            GestureModel.MotionModel newMotion = GestureModel.MotionModel.GetDefault(GestureModel.MotionModel.GetNewId(SelectedListGestureMotions.ToList<GestureModel.MotionModel>()));
            SelectedListGestureMotions.Add(newMotion);
        }

        private void DeleteGesture(object obj)
        {
            if (obj == null)
            {
                return;
            }

            Guid deleteID = (Guid)obj;

            foreach (GestureModel gesture in ListGesture)
            {
                if (gesture.ID.Equals(deleteID))
                {
                    GestureModel.Delete(gesture);
                    ListGesture.Remove(gesture);
                    break;
                }
            }
        }

        private void HandHandler(object obj)
        {
            CommunicationManager.GetInstance().ExecuteTheGesture("Сжать");
            //Communication.SaveGestures(ListGesture);
            // CommunicationManager.GetInstance().ExecuteTheGesture("Сжать");
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
