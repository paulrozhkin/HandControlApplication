﻿// --------------------------------------------------------------------------------------
// <copyright file = "MainWindowViewModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using HandControl.Model;
using HandControl.Services;
using HandControl.Services.ProstheticServices;
using HandControl.View;
using MaterialDesignThemes.Wpf;

namespace HandControl.ViewModel
{
    /// <summary>
    ///     Класс модель-представление главного окна программы. Предоставлет логику для работы с конфигурацией жестов протеза.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        /// <summary>
        ///     Поле искомого текста (имени жеста).
        /// </summary>
        private string _searchTextField;

        /// <summary>
        ///     Выбранный жест.
        /// </summary>
        private GestureModel _selectedGestureField;

        /// <summary>
        ///     Коллекция действий выбранного жеста <see cref="SelectedGesture" />.
        /// </summary>
        private ObservableCollection<GestureModel.ActionModel> _listMotionsField =
            new ObservableCollection<GestureModel.ActionModel>();

        private IGestureService _gestureService;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindowViewModel" /> class.
        /// </summary>
        public MainWindowViewModel(IProstheticManager prostheticManager, IGestureService gestureService)
        {
            Prosthetic = prostheticManager ?? throw new ArgumentNullException(nameof(prostheticManager));
            Prosthetic.IsConnectionChanged.Subscribe(ProstheticConnectionChangeHandler);
            _gestureService = gestureService ?? throw new ArgumentNullException(nameof(gestureService));

            gestureService.Gestures.ObserveOn(Application.Current.Dispatcher).Subscribe(GestureAddOrUpdateHandler);

            ListGesture = new ObservableCollection<GestureModel>();
            ListGestureView = CollectionViewSource.GetDefaultView(ListGesture);
        }

        private void GestureAddOrUpdateHandler(GestureModel gesture)
        {
            var gestureInUiResult = ListGesture.Where(x => x.Id == gesture.Id);
            var inUiResult = gestureInUiResult as GestureModel[] ?? gestureInUiResult.ToArray();
            if (inUiResult.Any())
            {
                var gestureInUi = inUiResult.Single();
                ListGesture.Remove(gestureInUi);
            }

            ListGesture.Add(gesture);
            SortGestures(ListGesture, 0, ListGesture.Count() - 1);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets состояние подключения протеза.
        /// </summary>
        public bool IsConnected { get; private set; }

        /// <summary>
        ///     Gets or sets интерфейс для управления протезом руки.
        /// </summary>
        public IProstheticManager Prosthetic { get; set; }

        /// <summary>
        ///     Gets or sets Список жестов протеза.
        /// </summary>
        public ObservableCollection<GestureModel> ListGesture { get; set; }

        /// <summary>
        ///     Gets or sets управление отображением коллекции <see cref="ListGesture" />.
        /// </summary>
        public ICollectionView ListGestureView { get; set; }

        /// <summary>
        ///     Gets or sets коллекция действий выбранного жеста <see cref="SelectedGesture" />.
        /// </summary>
        public ObservableCollection<GestureModel.ActionModel> SelectedListMotions
        {
            get => _listMotionsField;

            set
            {
                _listMotionsField = new ObservableCollection<GestureModel.ActionModel>();
                if (value != null)
                    foreach (var action in value)
                        _listMotionsField.Add((GestureModel.ActionModel) action.Clone());

                SelectedAction = null;
            }
        }

        /// <summary>
        ///     Gets or sets жест, выбранный пользователем в графическом интерфейсе.
        /// </summary>
        public GestureModel SelectedGesture
        {
            get => _selectedGestureField;

            set
            {
                if (value != null)
                {
                    _selectedGestureField = value.Clone() as GestureModel;
                    SelectedListMotions =
                        new ObservableCollection<GestureModel.ActionModel>(_selectedGestureField.ListMotions);
                }
                else
                {
                    SelectedListMotions = null;
                    _selectedGestureField = null;
                }
            }
        }

        /// <summary>
        ///     Gets or sets предполагаемое имя жеста, искомого пользователем.
        ///     Выполняет управлением отображением <see cref="ListGesture" /> с помощью <see cref="ListGestureView" />.
        /// </summary>
        public string SearchText
        {
            get => _searchTextField;

            set
            {
                _searchTextField = value;

                ListGestureView.Filter = obj =>
                {
                    if (obj is GestureModel gesture) return gesture.Name.ToLower().Contains(_searchTextField.ToLower());

                    return false;
                };

                ListGestureView.Refresh();
            }
        }

        /// <summary>
        ///     Gets or sets выбранное пользователем действие в <see cref="SelectedListMotions" />.
        /// </summary>
        public GestureModel.ActionModel SelectedAction { get; set; }

        #endregion

        #region Commands

        /// <summary>
        ///     Gets команду сохранения жеста.
        /// </summary>
        public ICommand SaveGestureCommand
        {
            get { return new RelayCommand(obj => SaveGesture()); }
        }

        /// <summary>
        ///     Gets команду добавления действия.
        /// </summary>
        public ICommand AddMotionCommand
        {
            get { return new RelayCommand(obj => AddMotion()); }
        }

        /// <summary>
        ///     Gets команду удаления жеста.
        /// </summary>
        public ICommand DeleteGestureCommand
        {
            get { return new RelayCommand(obj => DeleteGesture((Guid) obj)); }
        }

        /// <summary>
        ///     Gets команду удаления действия.
        /// </summary>
        public ICommand DeleteMotionCommand
        {
            get { return new RelayCommand(obj => DeleteMotion((int) obj)); }
        }

        /// <summary>
        ///     Gets команду удаления жеста.
        /// </summary>
        public ICommand ShowMioSettingsCommand
        {
            get { return new RelayCommand(obj => ShowMioSettingsAsync()); }
        }

        /// <summary>
        ///     Gets команду добавления жеста.
        /// </summary>
        public ICommand AddGestureCommand
        {
            get { return new RelayCommand(obj => AddGesture()); }
        }

        /// <summary>
        ///     Gets команду смены состояния бесконечного повторения жеста.
        /// </summary>
        public ICommand InfinityCheckCommand
        {
            get { return new RelayCommand(obj => InfinityGestureChangeState()); }
        }

        /// <summary>
        ///     Gets команду закрытия отображения жеста.
        /// </summary>
        public ICommand CloseChangeCommand
        {
            get { return new RelayCommand(obj => CloseChange()); }
        }

        #endregion

        #region Methods

        private void ProstheticConnectionChangeHandler(bool isConnected)
        {
            IsConnected = isConnected;
        }

        /// <summary>
        ///     Создания нового жеста.
        /// </summary>
        private void AddGesture()
        {
            var newGesture = GestureModel.GetDefault(Guid.NewGuid(), GetNewNameGesture());
            ListGesture.Add(newGesture);
            SortGestures(ListGesture, 0, ListGesture.Count() - 1);
            SelectedGesture = newGesture;
        }

        /// <summary>
        ///     Получить имя нового жеста.
        ///     Имя состоит из "Новый жест " + @id жеста, которое формируется на основании
        ///     имен жестов находящихся в <see cref="ListGesture" />.
        /// </summary>
        /// <returns>Новое имя жеста.</returns>
        private string GetNewNameGesture()
        {
            var list = ListGesture.Where(itemGesture => itemGesture.Name.Contains("Новый жест"));
            var maxValue = 1;

            if (list.Any())
            {
                foreach (var itemGesture in list)
                {
                    var nameGest = itemGesture.Name.Replace("Новый жест ", string.Empty);
                    try
                    {
                        var value = Convert.ToInt32(nameGest);
                        if (maxValue <= value) maxValue = value + 1;
                    }
                    catch (FormatException)
                    {
                    }
                }
            }

            return "Новый жест " + maxValue;
        }

        /// <summary>
        ///     Сохранение жеста. Выполняет обновление жеста в <see cref="ListGesture" /> и добавление в репозиторий
        ///     <see cref="_gestureRepositoryField" />.
        /// </summary>
        private async void SaveGesture()
        {
            SelectedGesture.ListMotions = SelectedListMotions.ToList();
            SelectedGesture.InfoGesture.TimeChange = DateTime.Now;

            try
            {
                await _gestureService.AddGestureAsync(SelectedGesture);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка сохранения жеста. {e.Message}");
                return;
            }

            var stateFountGesture = false;
            for (var i = 0; i < ListGesture.Count; i++)
                if (ListGesture[i].Id == SelectedGesture.Id)
                {
                    ListGesture[i] = SelectedGesture.Clone() as GestureModel;
                    stateFountGesture = true;
                    break;
                }

            if (stateFountGesture is false) ListGesture.Add(SelectedGesture.Clone() as GestureModel);

            SortGestures(ListGesture, 0, ListGesture.Count() - 1);
        }

        /// <summary>
        ///     Удаление жеста из системы по Id. Выполняет удаление жеста из <see cref="ListGesture" /> и
        ///     <see cref="_gestureRepositoryField" />.
        /// </summary>
        /// <param name="id">Id удаляемого жеста.</param>
        private async void DeleteGesture(Guid id)
        {
            var gesture = ListGesture.FirstOrDefault(gestureItem => gestureItem.Id.Equals(id));

            try
            {
                await _gestureService.RemoveGestureAsync(gesture);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Ошибка удаления жеста. {e.Message}");
                return;
            }

            ListGesture.Remove(gesture);
            SortGestures(ListGesture, 0, ListGesture.Count() - 1);
        }

        /// <summary>
        ///     Удалить действия в <see cref="SelectedListMotions" />.
        /// </summary>
        /// <param name="idDelMotion">Id действия.</param>
        private void DeleteMotion(int idDelMotion)
        {
            foreach (var actionModel in SelectedListMotions)
            {
                if (actionModel.Id == idDelMotion)
                {
                    for (var i = actionModel.Id; i < SelectedListMotions.Count; i++)
                        SelectedListMotions[i].Id = SelectedListMotions[i].Id - 1;

                    SelectedListMotions.Remove(actionModel);
                    break;
                }
            }
        }

        /// <summary>
        ///     Создание нового действия жеста. Выполняет создание нового экземпляра <see cref="GestureModel.ActionModel" /> и
        ///     добавление его к <see cref="SelectedListMotions" />.
        /// </summary>
        private void AddMotion()
        {
            var newMotion = GestureModel.ActionModel.GetDefault(
                GestureModel.ActionModel.GetNewId(SelectedListMotions.ToList()));
            SelectedListMotions.Add(newMotion);
        }

        /// <summary>
        ///     Обработчик действия с протезом руки. Пока что, возможно, нужен для тестов.
        ///     //TODO: потом убрать
        /// </summary>
        private async void ShowMioSettingsAsync()
        {
            //var gestures = await Prosthetic.GetGestures();
            ////ProstheticManager.GetInstance().ExecuteTheGesture("Сжать");
            ////Prosthetic.SaveGestures(ListGesture);
            ////ProstheticManager.GetInstance().ExecuteTheGesture("Сжать");
            ///

            if (!IsConnected)
                return;

            var view = new MioPatternsSettingsView()
            {
                DataContext = new MioPatternsSettingsViewModel(ListGesture.ToList(), Prosthetic.MioPatternsService)
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog");
        }

        /// <summary>
        ///     Выполняет изменение состояния итеративного выполнения выбранного жеста <see cref="SelectedGesture" />.
        /// </summary>
        private void InfinityGestureChangeState()
        {
            SelectedGesture.InfoGesture.IterableGesture = !SelectedGesture.InfoGesture.IterableGesture;
        }

        /// <summary>
        ///     Выполняет закрытие изменения выбранного жеста <see cref="SelectedGesture" /> путем его сброса в null.
        /// </summary>
        private void CloseChange()
        {
            SelectedGesture = null;
            SelectedAction = null;
        }

        /// <summary>
        ///     Выполняет сортировку переданной коллекции <see cref="GestureModel" /> по имени жеста в алфовитном порядке.
        /// </summary>
        /// <param name="collectionGustures">Сортируемоая коллекция.</param>
        /// <param name="start">Индекс начала сортировки.</param>
        /// <param name="end">Индекс конца сортировки.</param>
        private static void SortGestures(ObservableCollection<GestureModel> collectionGustures, int start, int end)
        {
            int Partition(ObservableCollection<GestureModel> collectionGusturesPartition, int startPartition,
                int endPartition)
            {
                GestureModel temp; //swap helper
                var marker = startPartition; //divides left and right subarrays
                for (var i = startPartition; i < endPartition; i++)
                {
                    if (NeedToReOrder(collectionGusturesPartition[endPartition].Name,
                        collectionGusturesPartition[i].Name)) //array[end] is pivot
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
                for (var i = 0; i < (s1.Length > s2.Length ? s2.Length : s1.Length); i++)
                {
                    if (s1[i] < s2[i]) return false;
                    if (s1[i] > s2[i]) return true;
                }

                return false;
            }

            if (start >= end)
            {
                return;
            }

            var pivot = Partition(collectionGustures, start, end);
            SortGestures(collectionGustures, start, pivot - 1);
            SortGestures(collectionGustures, pivot + 1, end);
        }

        #endregion
    }
}