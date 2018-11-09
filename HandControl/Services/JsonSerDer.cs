using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HandControl.Services
{
    public static class JsonSerDer
    {
        public static bool SaveObject(object obj, string path)
        {
            try
            {
                FileIOManager.WriteData(path, JsonConvert.SerializeObject(obj));
                return true;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
                return false;
            }
        }

        public static object LoadObject<T>(string path)
        {
            try
            {
                if (FileIOManager.ReadData(path) == "")
                {
                    return null;
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(FileIOManager.ReadData(path));
                }                
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                return null;
            }

        }
       

    }
}
