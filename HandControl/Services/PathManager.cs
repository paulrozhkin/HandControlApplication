﻿using System.Collections.Generic;
using System.IO;

namespace HandControl.Services
{
    public static class PathManager
    {
        public static readonly string MainDataPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\Data\\";
        private static readonly string FileGesturesName = "\\gesture.json";
        private static readonly string FolderGesturesName = "Gestures\\";
        private static readonly string FolderConfig = "config\\";
        private static readonly string FolderIODeviceConfig = "IODeviceConfig\\";

        public static string Txt = ".txt";
        public static string Json = ".json";

        public static List<string> GetGesturesFilesPaths()
        {
            List<string> gestures = new List<string>();
            string startPath = MainDataPath + FolderGesturesName;
            if (!Directory.Exists(startPath))
                Directory.CreateDirectory(startPath);
            string[] dirs = Directory.GetDirectories(startPath);
            foreach (var item in dirs)
            {
                string p = item + FileGesturesName;
                if (File.Exists(p))
                {
                    gestures.Add(p);
                }
            }
            return gestures;
        }

        public static string GetGesturePath(string idGesture)
        {
            string gestureFolderPath = GetGestureFolderPath(idGesture);

            string gesturePath = gestureFolderPath + "\\" + FileGesturesName;
            if (!File.Exists(gesturePath))
            {
                File.Create(gesturePath).Close();
            }
            return gesturePath;
        }

        public static string GetGestureFolderPath(string idGesture)
        {
            string gestureFolderPath = MainDataPath + FolderGesturesName + idGesture + "\\";
            if (!Directory.Exists(gestureFolderPath))
                Directory.CreateDirectory(gestureFolderPath);

            return gestureFolderPath;
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
