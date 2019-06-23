// --------------------------------------------------------------------------------------
// <copyright file = "IIODevice.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------
namespace HandControl.Services
{
    using System.ComponentModel;

    /// <summary>
    /// Интерфейс определяющий основные методы требующие имплементации для функций примо-передающих устройств системы.
    /// \brief Интерфейс для ввода-вывода данных системы.
    /// \version 1.0
    /// \date Январь 2019 года
    /// \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public interface IIODevice : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether cостояние устройства ввода-вывода.
        /// </summary>
        bool StateDeviceHand { get; }

        /// <summary>
        /// Выполняет подключение к устройству.
        /// </summary>
        /// <returns></returns>
        void ConnectDevice();

        /// <summary>
        /// Выполняет отключение от устройству.
        /// </summary>
        void DisconnectDevice();


        /// <summary>
        /// Отправка данных на устройство.
        /// </summary>
        /// <param name="dataTx">Отправляемые данные в byte</param>
        /// <returns>Состояние отправки.</returns>
        void SendToDevice(byte[] dataTx);

        /// <summary>
        /// Прием данных с устройства.
        /// </summary>
        /// <param name="commandRx">Команда по которой устройство определяет возвращаемые данные.</param>
        /// <returns>Принятые данные, содержащие ответ на команду.</returns>
        byte[] ReceiveFromDevice(byte commandRx);
    }
}
