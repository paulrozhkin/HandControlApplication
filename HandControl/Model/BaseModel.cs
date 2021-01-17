// --------------------------------------------------------------------------------------
// <copyright file = "BaseModel.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HandControl.Model
{
    /// <summary>
    ///     BaseModel - базовая имплементация компонента Model паттерна MVVM.
    ///     Класс содержит в себе имплементацию интерфейса INotifyPropertyChanged для оповещения об изменениях.
    ///     Все Model должны насследоваться от этого класса.
    ///     \brief базовая имплементация компонента Model паттерна MVVM.
    ///     \version 1.0
    ///     \date Апрель 2019 года
    ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    [Serializable]
    public abstract class BaseModel : INotifyPropertyChanged
    {
#pragma warning disable
        /// <summary>
        ///     Имплементация интерфейса INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}