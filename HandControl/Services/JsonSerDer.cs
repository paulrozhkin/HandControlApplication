using System;
using System.Windows;
using Newtonsoft.Json;

namespace HandControl.Services
{
    public static class JsonSerDer
    {
        public static bool SaveObject(object obj, string path)
        {
            try
            {
                FileSystemFacade.WriteData(path, JsonConvert.SerializeObject(obj));
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
                if (FileSystemFacade.ReadData(path) == "")
                    return null;
                return JsonConvert.DeserializeObject<T>(FileSystemFacade.ReadData(path));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }
        }
    }
}