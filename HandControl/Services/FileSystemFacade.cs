// --------------------------------------------------------------------------------------
// <copyright file = "FileSystemFacade.cs" company = "Студенческий проект HandControl‎"> 
//      Copyright © 2019 HandControl. All rights reserved.
// </copyright> 
// -------------------------------------------------------------------------------------

using System;
using System.IO;
using System.Windows;

namespace HandControl.Services
{
    /// <summary>
    ///     Класс, предоставляющий упрощенный доступ к файловой системе ПК.
    /// </summary>
    public static class FileSystemFacade
    {
        /// <summary>
        ///     Выполняет чтение строковых данных из файла.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Строки, содержащиеся в файле.</returns>
        public static string ReadData(string path)
        {
            var str = string.Empty;
            try
            {
                using (var rdr = new StreamReader(path))
                {
                    str = rdr.ReadToEnd();
                }

                return str;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                throw;
            }
        }

        /// <summary>
        ///     Выполняет запись строковых данных в файла.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="data">Строка для записи.</param>
        public static void WriteData(string path, string data)
        {
            using (var wrt = new StreamWriter(path))
            {
                wrt.Write(data);
            }
        }

        /// <summary>
        ///     Выполняет чтение бинарных данных из файла.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Поток байт, содержащихся в файле.</returns>
        public static byte[] ReadBinaryData(string path)
        {
            var data = File.ReadAllBytes(path);
            return data;
        }

        /// <summary>
        ///     Выполняет запись бинарного потока байт в файл.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="binaryData">Поток байт для записи.</param>
        public static void WriteBinaryData(string path, byte[] binaryData)
        {
            File.WriteAllBytes(path, binaryData);
        }

        /// <summary>
        ///     Удаление файла.
        ///     При ошибке выполнения выдает исключение.
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        public static void DeleteFile(string path)
        {
            File.Delete(path);
        }

        /// <summary>
        ///     Удаление директории из файловой системы.
        /// </summary>
        /// <param name="path">Путь к удаляемой директории.</param>
        public static void DeleteFolder(string path)
        {
            Directory.Delete(path, true);
        }
    }
}