// --------------------------------------------------------------------------------------
// <copyright file = "MainWindowViewModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;
    using HandControl.Model;
    using HandControl.Model.Repositories;
    using HandControl.Model.Repositories.GestureRepositories;
    using HandControl.Model.Repositories.GestureRepositories.Specifications;
    using HandControl.Services;

    class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        /// <summary>
        /// Поле искомого текста (имени жеста).
        /// </summary>
        private string searchTextField;

        /// <summary>
        /// Выбранный жест.
        /// </summary>
        private GestureModel selectedGestureField;
       
        /// <summary>
        /// Репозиторий жестов системы.
        /// </summary>
        private IGestureRepository gestureRepositoryField;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Communication = CommunicationManager.GetInstance();
            Communication.СonnectedDevices.ConnectDevice();

            IEntitySpecification<GestureModel> specGetByAll = new GesturesSpecificationByAll();

            gestureRepositoryField = new GestureRepositoryFactory().Create();
            ListGesture = new ObservableCollection<GestureModel>(gestureRepositoryField.Query(specGetByAll));
            ListGestureView = CollectionViewSource.GetDefaultView(ListGesture);

            // CommunicationManager.MotionListRequestCommand();
            // CommunicationManager.SaveCommandsToVoice(Commands);
            // CommunicationManager.SaveCommands(commands);
            // CommunicationManager.ExecuteTheCommand("Сжать");
            // CommunicationManager.ExecuteTheCommand("ModeVoice");
            // CommunicationManager.ExecuteTheCommand(commands[0]);
            // CommunicationManager.ExecuteTheRaw(15000);
        }
        #endregion

        #region Properties
        public CommunicationManager Communication { get; set; }

        /// <summary>
        /// Список жестов протеза.
        /// </summary>
        public ObservableCollection<GestureModel> ListGesture { get; set; }

        private ObservableCollection<GestureModel.MotionModel> listGestureMotionsField = new ObservableCollection<GestureModel.MotionModel>();

        public ICollectionView ListGestureView { get; set; }

        public ObservableCollection<GestureModel.MotionModel> SelectedListGestureMotions
        {
            get
            {
                return listGestureMotionsField;
            }
            set
            {
                listGestureMotionsField = new ObservableCollection<GestureModel.MotionModel>();
                if (value != null)
                {
                    foreach (GestureModel.MotionModel action in value)
                    {
                        listGestureMotionsField.Add((GestureModel.MotionModel)action.Clone());
                    }
                }
                SelectedMotion = null;
            }
        }

        public GestureModel SelectedGesture
        {
            get
            {
                return selectedGestureField;
            }
            set
            {
                if (value != null)
                {
                    selectedGestureField = value.Clone() as GestureModel;
                    SelectedListGestureMotions = new ObservableCollection<GestureModel.MotionModel>(selectedGestureField.ListMotions);
                }
                else
                {
                    SelectedListGestureMotions = null;
                    selectedGestureField = null;
                }
            }
        }

        public string SearchText
        {
            get
            {
                return searchTextField;
            }

            set
            {
                searchTextField = value;

                ListGestureView.Filter = (obj) =>
                {
                    if (obj is GestureModel patient)
                    {
                        return patient.Name.ToLower().Contains(searchTextField.ToLower());
                    }
                    return false;
                };

                ListGestureView.Refresh();
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

        #region Methods
        private void AddGesture(object obj)
        {
            GestureModel newGesture = GestureModel.GetDefault(Guid.NewGuid(), this.GetNewNameGesture());
            this.ListGesture.Add(newGesture);
            this.gestureRepositoryField.Add(newGesture);
        }

        private string GetNewNameGesture()
        {
            var list = this.ListGesture.Where(itemGesture => itemGesture.Name.Contains("Новый жест"));
            int maxValue = 1;

            if (list.Any())
            {
                foreach (GestureModel itemGesture in list)
                {
                    string nameGest = itemGesture.Name.Replace("Новый жест ", string.Empty);
                    try
                    {
                        int value = Convert.ToInt32(nameGest);
                        
                        if (maxValue <= value)
                        {
                            maxValue = value + 1;
                        }
                    }
                    catch (FormatException)
                    {
                        continue;
                    }
                }
            }

            return "Новый жест " + maxValue.ToString();

        }

        private void DeleteMotion(object obj)
        {
            if (obj == null)
            {
                return;
            }

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
            this.gestureRepositoryField.Add(SelectedGesture);
            ////GestureModel.Save(SelectedGesture);
            

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
            Guid id = (Guid)obj;
            GestureModel gesture = this.ListGesture.FirstOrDefault(gestureItem => gestureItem.ID.Equals(id));
            this.gestureRepositoryField.Remove(gesture);
            this.ListGesture.Remove(gesture);
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
