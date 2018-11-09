﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandControl.Services
{
    public interface IIODevice
    {
        /// <summary>
        /// Состояние устройства
        /// </summary>
        bool StateDevice { get; }

        /// <summary>
        /// Отправка данных на устройство.
        /// </summary>
        /// <param name="dataTx">Отправляемые данные в byte</param>
        /// <returns></returns>
        bool SendToDevice(byte[] dataTx);

        /// <summary>
        /// Прием данных с устройства.
        /// </summary>
        /// <param name="commandRx">Команда по которой устройство определяет возвращаемые данные</param>
        /// <returns></returns>
        byte[] ReceiveFromDevice(byte commandRx);
    }
}
