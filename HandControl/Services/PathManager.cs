using System.Collections.Generic;
using System.IO;

namespace HandControl.Services
{
    public static class PathManager
    {
        public static readonly string MainDataPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Data\\";
        private static readonly string FileCommandsName = "\\command.json";
        private static readonly string FolderCommandsName = "Commands\\";
        private static readonly string FolderConfig = "config\\";
        private static readonly string FolderIODeviceConfig = "IODeviceConfig\\";

        public static string Txt = ".txt";
        public static string Json = ".json";

        public static List<string> GetCommandsFilesPaths()
        {
            List<string> commands = new List<string>();
            string startPath = MainDataPath + FolderCommandsName;
            if (!Directory.Exists(startPath))
                Directory.CreateDirectory(startPath);
            string[] dirs = Directory.GetDirectories(startPath);
            foreach (var item in dirs)
            {
                string p = item + FileCommandsName;
                if (File.Exists(p))
                {
                    commands.Add(p);
                }
            }
            return commands;
        }

        public static string GetCommandPath(string NameCommand)
        {
            string commandFolderPath = MainDataPath + FolderCommandsName + NameCommand + "\\";
            if (!Directory.Exists(commandFolderPath))
                Directory.CreateDirectory(commandFolderPath);

            string sessionPath = commandFolderPath + "\\" + FileCommandsName;
            if (!File.Exists(sessionPath))
            {
                File.Create(sessionPath).Close();
            }
            return sessionPath;
        }

        public static string IODevicePath(string nameDevice)
        {
            string IODeviceFolderPath = MainDataPath + FolderConfig + FolderIODeviceConfig;
            if (!Directory.Exists(IODeviceFolderPath))
                Directory.CreateDirectory(IODeviceFolderPath);
            string configFilePath = IODeviceFolderPath + "IODevice" + nameDevice + ".conf";
            if (!File.Exists(configFilePath))
            {
                File.Create(configFilePath).Close();
            }

            return configFilePath;
        }
        
    }
}
