// --------------------------------------------------------------------------------------
// <copyright file = "IdToNameMotionConverter.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------
namespace HandControl.Services
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Класс содержащий конвертер преобразующий Id действия в строковое представление.
    /// \version 1.0
    /// \date Январь 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public class IdToNameMotionConverter : IValueConverter
    {
        /// <summary>
        /// Конвертиация id в строковое представление путем добавления слова Действие в начало строки.
        /// </summary>
        /// <param name="value">Конвертируемое значние.</param>
        /// <param name="targetType">Ожидаемый тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Сведения о языке системы.</param>
        /// <returns>'Действие ' + id</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Действие " + (int)value;
        }

        /// <summary>
        /// Обратная конвертация значения, не требуется в системе.
        /// </summary>
        /// <param name="value">Конвертируемое значние.</param>
        /// <param name="targetType">Ожидаемый тип.</param>
        /// <param name="parameter">Параметр конвертера.</param>
        /// <param name="culture">Сведения о языке системы.</param>
        /// <returns>Id действия</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
