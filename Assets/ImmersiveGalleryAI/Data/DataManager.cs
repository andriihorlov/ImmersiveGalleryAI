using System.Collections.Generic;
using ImmersiveGalleryAI.Utilities;
using UnityEngine;

namespace ImmersiveGalleryAI.Data
{
    public class DataManager : IDataManager
    {
        public SettingsData Settings { get; private set; }

        public void SaveSettings()
        {
            FileManager.SaveSettings(Settings);
        }

        public void DeleteImage(ImageData imageData)
        {
            Settings.ImagesData.Remove(imageData);
            FileManager.DeleteImage(imageData.FilePath);
        }

        public void SaveImage(ImageData imageData)
        {
            ImageData newImageData = FileManager.SaveImage(imageData.WallId, imageData.FileContent);
            AddImageInSettings(newImageData);
        }

        public void ShareImage(ImageData imageData)
        {
            //todo: will be implemented in future
            Debug.Log($"Shared! {imageData.FileName}");
        }

        public void LoadSettings()
        {
            Settings = FileManager.LoadSettings();
        }

        private void AddImageInSettings(ImageData imageData)
        {
            Settings ??= new SettingsData();
            Settings.ImagesData ??= new List<ImageData>();
            Settings.ImagesData.Add(imageData);
        }
    }
}