// --------------------------------------------------------------------------------------
// <copyright file = "IIoDevice.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright>
// -------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using HandControl.Model;
using HandControl.Model.BluetoothDto;
using HandControl.Model.Enums;

namespace HandControl.Services.IODevice
{
    /// <summary>
    ///     Интерфейс определяющий основные методы требующие имплементации для функций примо-передающих устройств системы.
    ///     \brief Интерфейс для ввода-вывода данных системы.
    ///     \version 1.0
    ///     \date Январь 2019 года
    ///     \authors Paul Rozhkin(blackiiifox@gmail.com)
    /// </summary>
    public interface IIoDevice
    {
        /// <summary>
        ///     Gets a value indicating whether cостояние устройства ввода-вывода.
        /// </summary>
        IObservable<bool> IsConnectedStatusChanged { get; }

        /// <summary>
        /// Gets a value indicating whether телеметрию устройства.
        /// </summary>
        IObservable<PackageDto> TelemetryPackages { get; }

        /// <summary>
        ///     Выполняет подключение к устройству.
        /// </summary>
        /// <returns></returns>
        void ConnectDevice();

        /// <summary>
        ///     Выполняет отключение от устройству.
        /// </summary>
        void DisconnectDevice();

        /// <summary>
        ///     Отправка данных на устройство.
        /// </summary>
        /// <param name="command">Команда протеза.</param>
        /// <param name="payload">Отправляемые данные в byte</param>
        /// <returns>Ответ с протеза.</returns>
        Task<byte[]> SendToDevice(CommandType command, byte[] payload);

        /// <summary>
        ///     Отправка данных на устройство.
        /// </summary>
        /// <param name="command">Команда протеза.</param>
        /// <returns>Ответ с протеза.</returns>
        Task<byte[]> SendToDevice(CommandType command);
    }
}