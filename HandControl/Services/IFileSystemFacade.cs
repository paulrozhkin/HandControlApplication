namespace HandControl.Services
{
    public interface IFileSystemFacade
    {
        /// <summary>
        ///     Выполняет чтение строковых данных из файла.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Строки, содержащиеся в файле.</returns>
        string ReadData(string path);

        /// <summary>
        ///     Выполняет запись строковых данных в файла.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="data">Строка для записи.</param>
        void WriteData(string path, string data);

        /// <summary>
        ///     Выполняет чтение бинарных данных из файла.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Поток байт, содержащихся в файле.</returns>
        byte[] ReadBinaryData(string path);

        /// <summary>
        ///     Выполняет запись бинарного потока байт в файл.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="binaryData">Поток байт для записи.</param>
        void WriteBinaryData(string path, byte[] binaryData);

        /// <summary>
        ///     Удаление файла.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        void DeleteFile(string path);

        /// <summary>
        ///     Удаление директории из файловой системы.
        /// </summary>
        /// <param name="path">Путь к удаляемой директории.</param>
        void DeleteFolder(string path);
    }
}