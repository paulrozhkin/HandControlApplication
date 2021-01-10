using System;
using System.Collections.Generic;
using System.IO;

namespace HandControl.Services
{
    public static class PathManager
    {
        private static readonly string MainDataPath = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\";
        private const string FileGesturesName = "\\gesture.bin";
        private const string FolderGesturesName = "Gestures\\";
        private const string FolderConfig = "config\\";
        private const string FolderIoDeviceConfig = "IODeviceConfig\\";
        private const string FileGesturesInfoName = "gesturesInfo.json";

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

        public static string GetGestureInfoPath()
        {
            var gesturesPaths = $"{MainDataPath}{FolderGesturesName}";
            if (!Directory.Exists(gesturesPaths))
            {
                Directory.CreateDirectory(gesturesPaths);
            }

            var configFilePath = $"{gesturesPaths}{FileGesturesInfoName}";
            if (!File.Exists(configFilePath))
            {
                File.Create(configFilePath).Close();
            }

            return configFilePath;
        }

        public static string IoDevicePath(string nameDevice)
        {
            var ioDeviceFolderPath = MainDataPath + FolderConfig + FolderIoDeviceConfig;
            if (!Directory.Exists(ioDeviceFolderPath))
                Directory.CreateDirectory(ioDeviceFolderPath);
            var configFilePath = ioDeviceFolderPath + "IODevice" + nameDevice + ".conf";
            if (!File.Exists(configFilePath)) File.Create(configFilePath).Close();

            return configFilePath;
        }
    }
}