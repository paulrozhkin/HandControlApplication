namespace HandControl.Model.Enums
{
    /// <summary>
    ///     Определяет команды, реализуемые протезом.
    /// </summary>
    public enum CommandType
    {
        /// <summary>
        ///     Успешное выполнение команды
        /// </summary>
        Ack = 0x01,

        /// <summary>
        ///     Ошибка выполнения команды
        /// </summary>
        Error = 0x02,

        /// <summary>
        ///     Телеметрия устройства
        /// </summary>
        Telemetry = 0x03,

        /// <summary>
        ///     Получение настроек протеза
        /// </summary>
        GetSettings = 0x04,


        /// <summary>
        ///     Установка настроек на протез
        /// </summary>
        SetSettings = 0x05,

        /// <summary>
        ///     Получение всех жестов с протеза.
        /// </summary>
        GetGestures = 0x06,

        /// <summary>
        ///     Сохранение жеста протеза.
        /// </summary>
        SaveGestures = 0x07,

        DeleteGestures = 0x08,

        /// <summary>
        ///     Исполнение жеста по id.
        /// </summary>
        PerformGestureId = 0x09,

        /// <summary>
        ///     Исполнение жеста.
        /// </summary>
        PerformGestureRaw = 0x10,

        /// <summary>
        ///     Установки переданных положений пальцев на протез.
        /// </summary>
        SetPositions = 0x11
    }
}