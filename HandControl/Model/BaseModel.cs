// --------------------------------------------------------------------------------------
// <copyright file = "BaseModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Model
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// BaseModel - базовая имплементация компонента Model паттерна MVVM.
    /// Класс содержит в себе имплементацию интерфейса INotifyPropertyChanged для оповещения об изменениях.
    /// Все Model должны насследоваться от этого класса.
    /// \brief базовая имплементация компонента Model паттерна MVVM.
    /// \version 1.0
    /// \date Апрель 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    [Serializable]
    public abstract class BaseModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Имплементация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
