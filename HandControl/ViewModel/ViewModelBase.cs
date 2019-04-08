// --------------------------------------------------------------------------------------
// <copyright file = "ViewModelBase.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.ViewModel
{
    using System.ComponentModel;

    /// <summary>
    /// ViewModelBase - базовая имплементация ViewModel.
    /// Все ViewModels должны насследоваться от этого класса.
    /// \brief Класс для работы с данными системы.
    /// \version 1.0
    /// \date Март 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Имплементация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
