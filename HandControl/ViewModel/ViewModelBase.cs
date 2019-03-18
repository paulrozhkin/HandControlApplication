using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandControl.ViewModel
{
    /// <summary>
    /// Базовый клас модели представления окон программы.
    /// Все ViewModel классы (за исключением диалогов) должны насследоваться от этого класса.
    /// </summary>
    class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Реализация интерфейса INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
