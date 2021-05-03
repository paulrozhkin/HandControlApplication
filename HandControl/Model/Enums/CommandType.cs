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
        Ack = 1,

        /// <summary>
        ///     Ошибка выполнения команды
        /// </summary>
        Error = 2,

        /// <summary>
        ///     Телеметрия устройства
        /// </summary>
        Telemetry = 3,

        /// <summary>
        ///     Получение настроек протеза
        /// </summary>
        GetSettings = 4,


        /// <summary>
        ///     Установка настроек на протез
        /// </summary>
        SetSettings = 5,

        /// <summary>
        ///     Получение всех жестов с протеза.
        /// </summary>
        GetGestures = 6,

        /// <summary>
        ///     Сохранение жеста протеза.
        /// </summary>
        SaveGestures = 7,

        DeleteGestures = 8,

        /// <summary>
        ///     Исполнение жеста по id.
        /// </summary>
        PerformGestureId = 9,

        /// <summary>
        ///     Исполнение жеста.
        /// </summary>
        PerformGestureRaw = 10,

        /// <summary>
        ///     Установка переданных положений пальцев на протез.
        /// </summary>
        SetPositions = 11,

        /// <summary>
        ///     Обновить время синхронизации жестов на протезе.
        /// </summary>
        UpdateLastTimeSync = 12,

        /// <summary>
        ///     Текущая телеметрия протеза.
        /// </summary>
        GetTelemetry = 13,

        /// <summary>
        ///     Начать отправку телеметрии на протезе.
        /// </summary>
        StartTelemetry = 14,

        /// <summary>
        ///     Остановить отправку телеметрии на протезе.
        /// </summary>
        StopTelemetry = 15,

        /// <summary>
        ///     Получить настройки паттернов протеза.
        /// </summary>
        GetMioPatterns = 16,

        /// <summary>
        ///     Установить настройки паттернов протеза.
        /// </summary>
        SetMioPatterns = 17,
    }
}