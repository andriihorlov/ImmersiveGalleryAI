using System;
using System.IO;
using ImmersiveGalleryAI.Data;
using ImmersiveGalleryAI.ImageHandler;
using UnityEngine;

namespace ImmersiveGalleryAI.Utilities
{
    public static class FileManager
    {
        private static readonly string DefaultFolder = Application.persistentDataPath;
        private const string SettingsFileName = "/Settings";

        private const string ImageFolder = "/AppData/";
        private const string ImageFormat = ".jpg";
        private const string ImageDefaultName = "Img";

        private static readonly string FilePathFormat = $"{DefaultFolder}{ImageFolder}";

        public static ImageData SaveImage(int wallId, byte[] content)
        {
            string fileName = ImageDefaultName + DateTime.Now.ToFileTime();
            ImageData imageData = new ImageData {WallId = wallId, FileContent = content, FileName = fileName};
            SaveImage(imageData);
            return imageData;
        }

        public static SettingsData LoadSettings()
        {
            string settingsJson = Load(DefaultFolder + SettingsFileName);
            return GetModelData<SettingsData>(settingsJson);
        }

        public static void DeleteImage(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"File deleted!\r\n {filePath}");
            }
        }

        public static void SaveSettings(SettingsData settingsData)
        {
            string filePath = DefaultFolder + SettingsFileName;
            CreateFileDirectoryIfNeeded(filePath);

            string jsonData = GetJsonData(settingsData);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(jsonData);
            streamWriter.Close();
            Debug.Log("<color=red>Settings changed!</color>");
        }
        
        private static void SaveImage(ImageData jsonData)
        {
            string filePath = FilePathFormat + jsonData.FileName + ImageFormat;
            jsonData.FilePath = filePath;
            CreateFileDirectoryIfNeeded(filePath);
            File.WriteAllBytes(filePath, jsonData.FileContent);
        }
        
        private static string Load(string pathFile)
        {
            if (!File.Exists(pathFile))
            {
                return null;
            }

            StreamReader streamReader = File.OpenText(pathFile);
            string fileData = streamReader.ReadToEnd();
            streamReader.Close();
            return fileData;
        }

        private static void CreateFileDirectoryIfNeeded(string pathFile)
        {
            if (File.Exists(pathFile))
            {
                File.WriteAllText(pathFile, string.Empty);
                return;
            }

            string folderPath = Path.GetDirectoryName(pathFile);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            FileStream fileStream = File.Create(pathFile);
            fileStream.Close();
            fileStream.Dispose();
        }

        private static string GetJsonData<T>(T savedFile)
        {
            return JsonUtility.ToJson(savedFile);
        }

        private static T GetModelData<T>(string jsonData)
        {
            return JsonUtility.FromJson<T>(jsonData);
        }
    }
}