// --------------------------------------------------------------------------------------
// <copyright file = "MainWindowViewModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.ViewModel
{
    using System;
    using System.Collections.Generic;
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

    /// <summary>
    /// Класс модель-представление главного окна программы. Предоставлет логику для работы с конфигурацией жестов протеза.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
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

        /// <summary>
        /// Коллекция действий выбранного жеста <see cref="SelectedGesture"/>.
        /// </summary>
        private ObservableCollection<GestureModel.MotionModel> listMotionsField = new ObservableCollection<GestureModel.MotionModel>();
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel()
        {
            this.Communication = CommunicationManager.GetInstance();
            this.Communication.СonnectedDevices.ConnectDevice();

            IEntitySpecification<GestureModel> specGetByAll = new GesturesSpecificationByAll();

            this.gestureRepositoryField = new GestureRepositoryFactory().Create();
            this.ListGesture = new ObservableCollection<GestureModel>(this.gestureRepositoryField.Query(specGetByAll));
            this.ListGestureView = CollectionViewSource.GetDefaultView(this.ListGesture);

            SortGestures(this.ListGesture, 0, this.ListGesture.Count() - 1);

            //// CommunicationManager.MotionListRequestCommand();
            //// CommunicationManager.SaveCommandsToVoice(Commands);
            //// CommunicationManager.SaveCommands(commands);
            //// CommunicationManager.ExecuteTheCommand("Сжать");
            //// CommunicationManager.ExecuteTheCommand("ModeVoice");
            //// CommunicationManager.ExecuteTheCommand(commands[0]);
            //// CommunicationManager.ExecuteTheRaw(15000);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets интерфейс для управления протезом руки.
        /// </summary>
        public CommunicationManager Communication { get; set; }

        /// <summary>
        /// Gets or sets Список жестов протеза.
        /// </summary>
        public ObservableCollection<GestureModel> ListGesture { get; set; }

        /// <summary>
        /// Gets or sets управление отображением коллекции <see cref="ListGesture"/>.
        /// </summary>
        public ICollectionView ListGestureView { get; set; }

        /// <summary>
        /// Gets or sets коллекция действий выбранного жеста <see cref="SelectedGesture"/>.
        /// </summary>
        public ObservableCollection<GestureModel.MotionModel> SelectedListMotions
        {
            get
            {
                return this.listMotionsField;
            }

            set
            {
                this.listMotionsField = new ObservableCollection<GestureModel.MotionModel>();
                if (value != null)
                {
                    foreach (GestureModel.MotionModel action in value)
                    {
                        this.listMotionsField.Add((GestureModel.MotionModel)action.Clone());
                    }
                }

                this.SelectedMotion = null;
            }
        }

        /// <summary>
        /// Gets or sets жест, выбранный пользователем в графическом интерфейсе.
        /// </summary>
        public GestureModel SelectedGesture
        {
            get
            {
                return this.selectedGestureField;
            }

            set
            {
                if (value != null)
                {
                    this.selectedGestureField = value.Clone() as GestureModel;
                    this.SelectedListMotions = new ObservableCollection<GestureModel.MotionModel>(this.selectedGestureField.ListMotions);
                }
                else
                {
                    this.SelectedListMotions = null;
                    this.selectedGestureField = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets предполагаемое имя жеста, искомого пользователем.
        /// Выполняет управлением отображением <see cref="ListGesture"/> с помощью <see cref="ListGestureView"/>.
        /// </summary>
        public string SearchText
        {
            get
            {
                return this.searchTextField;
            }

            set
            {
                this.searchTextField = value;

                this.ListGestureView.Filter = (obj) =>
                {
                    if (obj is GestureModel gesture)
                    {
                        return gesture.Name.ToLower().Contains(this.searchTextField.ToLower());
                    }

                    return false;
                };

                this.ListGestureView.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets выбранное пользователем действие в <see cref="SelectedListMotions"/>.
        /// </summary>
        public GestureModel.MotionModel SelectedMotion { get; set; }
        #endregion

        #region Commands
        /// <summary>
        /// Gets команду сохранения жеста.
        /// </summary>
        public ICommand SaveGestureCommand
        {
            get { return new RelayCommand((object obj) => this.SaveGesture()); }
        }

        /// <summary>
        /// Gets команду добавления действия.
        /// </summary>
        public ICommand AddMotionCommand
        {
            get { return new RelayCommand((object obj) => this.AddMotion()); }
        }

        /// <summary>
        /// Gets команду удаления жеста.
        /// </summary>
        public ICommand DeleteGestureCommand
        {
            get { return new RelayCommand((object obj) => this.DeleteGesture((Guid)obj)); }
        }

        /// <summary>
        /// Gets команду удаления действия.
        /// </summary>
        public ICommand DeleteMotionCommand
        {
            get { return new RelayCommand((object obj) => this.DeleteMotion((int)obj)); }
        }

        /// <summary>
        /// Gets команду добавления жеста.
        /// </summary>
        public ICommand AddGestureCommand
        {
            get { return new RelayCommand((object obj) => this.AddGesture()); }
        }

        /// <summary>
        /// Gets команду обработки нажатия на кнопку работы с протезом.
        /// </summary>
        public ICommand HandCommand
        {
            get { return new RelayCommand((object obj) => this.HandHandler()); }
        }

        /// <summary>
        /// Gets команду смены состояния бесконечного повторения жеста.
        /// </summary>
        public ICommand InfinityCheckCommand
        {
            get { return new RelayCommand((object obj) => this.InfinityGestureChangeState()); }
        }

        /// <summary>
        /// Gets команду закрытия отображения жеста.
        /// </summary>
        public ICommand CloseChangeCommand
        {
            get { return new RelayCommand((object obj) => this.CloseChange()); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Создания новой команды.
        /// </summary>
        private void AddGesture()
        {
            GestureModel newGesture = GestureModel.GetDefault(Guid.NewGuid(), this.GetNewNameGesture());
            this.ListGesture.Add(newGesture);
            this.gestureRepositoryField.Add(newGesture);
            SortGestures(this.ListGesture, 0, this.ListGesture.Count() - 1);
            this.SelectedGesture = newGesture;
        }

        /// <summary>
        /// Получить имя нового жеста.
        /// Имя состоит из "Новый жест " + @id жеста, которое формируется на основании
        /// имен жестов находящихся в <see cref="ListGesture"/>.
        /// </summary>
        /// <returns>Новое имя жеста.</returns>
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

        /// <summary>
        /// Сохранение жеста. Выполняет обновление жеста в <see cref="ListGesture"/> и добавление в репозиторий <see cref="gestureRepositoryField"/>.
        /// </summary>
        private void SaveGesture()
        {
            this.SelectedGesture.ListMotions = this.SelectedListMotions.ToList();
            this.SelectedGesture.InfoGesture.TimeChange = DateTime.Now;
            this.SelectedGesture.InfoGesture.NumberOfMotions = this.SelectedListMotions.Count();
            this.gestureRepositoryField.Add(this.SelectedGesture);
            ////GestureModel.Save(SelectedGesture);

            bool stateFountGesture = false;
            for (int i = 0; i < this.ListGesture.Count; i++)
            {
                if (this.ListGesture[i].Id == this.SelectedGesture.Id)
                {
                    this.ListGesture[i] = this.SelectedGesture.Clone() as GestureModel;
                    stateFountGesture = true;
                    break;
                }
            }

            if (stateFountGesture is false)
            {
                this.ListGesture.Add(this.SelectedGesture.Clone() as GestureModel);
            }
            SortGestures(this.ListGesture, 0, this.ListGesture.Count() - 1);
        }

        /// <summary>
        /// Удаление жеста из системы по Id. Выполняет удаление жеста из <see cref="ListGesture"/> и <see cref="gestureRepositoryField"/>.
        /// </summary>
        /// <param name="id">Id удаляемого жеста.</param>
        private void DeleteGesture(Guid id)
        {
            GestureModel gesture = this.ListGesture.FirstOrDefault(gestureItem => gestureItem.Id.Equals(id));
            this.gestureRepositoryField.Remove(gesture);
            this.ListGesture.Remove(gesture);
            SortGestures(this.ListGesture, 0, this.ListGesture.Count() - 1);
        }

        /// <summary>
        /// Удалить действия в <see cref="SelectedListMotions"/>.
        /// </summary>
        /// <param name="idDelMotion">Id действия.</param>
        private void DeleteMotion(int idDelMotion)
        {
            foreach (GestureModel.MotionModel actionModel in this.SelectedListMotions)
            {
                if (actionModel.Id == idDelMotion)
                {
                    for (int i = actionModel.Id; i < this.SelectedListMotions.Count; i++)
                    {
                        this.SelectedListMotions[i].Id = this.SelectedListMotions[i].Id - 1;
                    }

                    this.SelectedListMotions.Remove(actionModel);
                    break;
                }
            }
        }

        /// <summary>
        /// Создание нового действия жеста. Выполняет создание нового экземпляра <see cref="GestureModel.MotionModel"/> и добавление его к <see cref="SelectedListMotions"/>.
        /// </summary>
        private void AddMotion()
        {
            GestureModel.MotionModel newMotion = GestureModel.MotionModel.GetDefault(GestureModel.MotionModel.GetNewId(this.SelectedListMotions.ToList<GestureModel.MotionModel>()));
            this.SelectedListMotions.Add(newMotion);
        }

        /// <summary>
        /// Обработчик действия с протезом руки. Пока что, возможно, нужен для тестов. 
        /// //TODO: потом убрать
        /// </summary>
        private void HandHandler()
        {
            ////CommunicationManager.GetInstance().ExecuteTheGesture("Сжать");
            ////Communication.SaveGestures(ListGesture);
            ////CommunicationManager.GetInstance().ExecuteTheGesture("Сжать");
        }

        /// <summary>
        /// Выполняет изменение состояния итеративного выполнения выбранного жеста <see cref="SelectedGesture"/>.
        /// </summary>
        private void InfinityGestureChangeState()
        {
            this.SelectedGesture.InfoGesture.IterableGesture = !this.SelectedGesture.InfoGesture.IterableGesture;
        }

        /// <summary>
        /// Выполняет закрытие изменения выбранного жеста <see cref="SelectedGesture"/> путем его сброса в <see cref="null"/>.
        /// </summary>
        private void CloseChange()
        {
            this.SelectedGesture = null;
            this.SelectedMotion = null;
        }

        /// <summary>
        /// Выполняет сортировку переданной коллекции <see cref="GestureModel"/> по имени жеста в алфовитном порядке.
        /// </summary>
        /// <param name="collectionGustures">Сортируемоая коллекция.</param>
        /// <param name="start">Индекс начала сортировки.</param>
        /// <param name="end">Индекс конца сортировки.</param>
        private static void SortGestures(ObservableCollection<GestureModel> collectionGustures, int start, int end)
        {
            int partition(ObservableCollection<GestureModel> collectionGusturesPartition, int startPartition, int endPartition)
            {
                GestureModel temp;//swap helper
                int marker = startPartition;//divides left and right subarrays
                for (int i = startPartition; i < endPartition; i++)
                {
                    if (NeedToReOrder(collectionGusturesPartition[endPartition].Name, collectionGusturesPartition[i].Name)) //array[end] is pivot
                    {
                        temp = collectionGusturesPartition[marker]; // swap
                        collectionGusturesPartition[marker] = collectionGusturesPartition[i];
                        collectionGusturesPartition[i] = temp;
                        marker += 1;
                    }
                }
                //put pivot(array[end]) between left and right subarrays
                temp = collectionGusturesPartition[marker];
                collectionGusturesPartition[marker] = collectionGusturesPartition[endPartition];
                collectionGusturesPartition[endPartition] = temp;
                return marker;
            }

            bool NeedToReOrder(string s1, string s2)
            {
                for (int i = 0; i < (s1.Length > s2.Length ? s2.Length : s1.Length); i++)
                {
                    if (s1.ToCharArray()[i] < s2.ToCharArray()[i]) return false;
                    if (s1.ToCharArray()[i] > s2.ToCharArray()[i]) return true;
                }
                return false;
            }

            if (start >= end)
            {
                return;
            }

            int pivot = partition(collectionGustures, start, end);
            SortGestures(collectionGustures, start, pivot - 1);
            SortGestures(collectionGustures, pivot + 1, end);
        }
        #endregion
    }
}
