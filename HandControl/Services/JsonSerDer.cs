using System;
using System.Windows;
using Newtonsoft.Json;

namespace HandControl.Services
{
    public static class JsonSerDer
    {
        // TODO: переписать
        private static IFileSystemFacade _fileSystemFacade = new FileSystemFacade();

        public static bool SaveObject(object obj, string path)
        {
            try
            {
                _fileSystemFacade.WriteData(path, JsonConvert.SerializeObject(obj));
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public static object LoadObject<T>(string path)
        {
            try
            {
                var content = _fileSystemFacade.ReadData(path);
                if (content == "")
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }
    }
}