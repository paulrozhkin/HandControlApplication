using System;
using System.Collections.Generic;
using System.IO;

namespace HandControl.Services
{
    public static class PathManager
    {
        public static readonly string MainDataPath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\";
        private static readonly string FileGesturesName = "\\gesture.json";
        private static readonly string FolderGesturesName = "Gestures\\";
        private static readonly string FolderConfig = "config\\";
        private static readonly string FolderIODeviceConfig = "IODeviceConfig\\";

        public static string Txt = ".txt";
        public static string Json = ".json";

        public static List<string> GetGesturesFilesPaths()
        {
            var gestures = new List<string>();
            var startPath = MainDataPath + FolderGesturesName;
            if (!Directory.Exists(startPath))
                Directory.CreateDirectory(startPath);
            var dirs = Directory.GetDirectories(startPath);
            foreach (var item in dirs)
            {
                var p = item + FileGesturesName;
                if (File.Exists(p)) gestures.Add(p);
            }

            return gestures;
        }

        public static string GetGesturePath(string idGesture)
        {
            var gestureFolderPath = GetGestureFolderPath(idGesture);

            var gesturePath = gestureFolderPath + "\\" + FileGesturesName;
            if (!File.Exists(gesturePath)) File.Create(gesturePath).Close();
            return gesturePath;
        }

        public static string GetGestureFolderPath(string idGesture)
        {
            var gestureFolderPath = MainDataPath + FolderGesturesName + idGesture + "\\";
            if (!Directory.Exists(gestureFolderPath))
                Directory.CreateDirectory(gestureFolderPath);

            return gestureFolderPath;
        }

        public static string IODevicePath(string nameDevice)
        {
            var IODeviceFolderPath = MainDataPath + FolderConfig + FolderIODeviceConfig;
            if (!Directory.Exists(IODeviceFolderPath))
                Directory.CreateDirectory(IODeviceFolderPath);
            var configFilePath = IODeviceFolderPath + "IODevice" + nameDevice + ".conf";
            if (!File.Exists(configFilePath)) File.Create(configFilePath).Close();

            return configFilePath;
        }
    }
}