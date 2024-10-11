using System;
using System.IO;
using ImmersiveGalleryAI.Main.ImageData;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.Utilities
{
    public static class FileManager
    {
        private static readonly string DefaultFolder = Application.persistentDataPath;
        private const string ImagesListFileName = "/Settings";
        private const string OpenAiSettingsFileName = "/OpenAiAPI.json";

        private const string ImageFolder = "/AppData/";
        private const string ImageFormat = ".jpg";
        private const string ImageDefaultName = "Img";

        private static readonly string FilePathFormat = $"{DefaultFolder}{ImageFolder}";

        public static ImageData SaveNewImage(ImageData imageData)
        {
            imageData.FileName = ImageDefaultName + DateTime.Now.ToFileTime();
            SaveImage(imageData);
            return imageData;
        }

        public static AllImages LoadAllImages()
        {
            string settingsJson = Load(DefaultFolder + ImagesListFileName);
            return GetModelData<AllImages>(settingsJson);
        }

        public static void DeleteImage(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"File deleted!\r\n {filePath}");
            }
        }

        public static void SaveImages(AllImages allImages)
        {
            string filePath = DefaultFolder + ImagesListFileName;
            CreateFileDirectoryIfNeeded(filePath);

            string jsonData = GetJsonData(allImages);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(jsonData);
            streamWriter.Close();
            Debug.Log("<color=red>All images saved!</color>");
        }

        public static string LoadLocalOpenAiApi()
        {
            string openAiFileName = DefaultFolder + OpenAiSettingsFileName;
            if (!File.Exists(openAiFileName))
            {
                CreateOpenAiApiFile(openAiFileName);
                return null;
            }

            string json = Load(openAiFileName);
            CustomOpenAiApi api = GetModelData<CustomOpenAiApi>(json);
            return api.privateApiKey;
        }

        private static void CreateOpenAiApiFile(string openAiFileName)
        {
            CreateFileDirectoryIfNeeded(openAiFileName);
            if (!File.Exists(openAiFileName))
            {
                Debug.LogError($"Something wrong with creation OpenAI api file");
                return;
            }

            CustomOpenAiApi authSettings = new CustomOpenAiApi {privateApiKey = string.Empty};
            string json = GetJsonData(authSettings);
            File.WriteAllText(openAiFileName, json);
            Debug.Log($"Open AI template file created");
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