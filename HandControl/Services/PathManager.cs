using System.Collections.Generic;
using System.IO;

namespace HandControl.Services
{
    public static class PathManager
    {
        public static readonly string MainDataPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Data\\";
        private static readonly string FileCommandsName = "\\command.json";
        private static readonly string FolderCommandsName = "Commands\\";

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
            string sessionFolderPath = MainDataPath + FolderCommandsName + "\\" + NameCommand + "\\";
            if (!Directory.Exists(sessionFolderPath))
                Directory.CreateDirectory(sessionFolderPath);

            string sessionPath = sessionFolderPath + "\\" + FileCommandsName;
            if (!File.Exists(sessionPath))
            {
                File.Create(sessionPath).Close();
            }
            return sessionPath;
        }
        
    }
}
