using System.Collections.Generic;
using ImmersiveGalleryAI.Common.Utilities;
using UnityEngine;

namespace ImmersiveGalleryAI.Main.ImageData
{
    public class ImageImageDataManager : IImageDataManager
    {
        public SettingsData Settings { get; private set; }

        public void SaveSettings()
        {
            FileManager.SaveSettings(Settings);
        }

        public void DeleteImage(Main.ImageData.ImageData imageData)
        {
            Settings.ImagesData.Remove(imageData);
            FileManager.DeleteImage(imageData.FilePath);
        }

        public void SaveImage(Main.ImageData.ImageData imageData)
        {
            Main.ImageData.ImageData newImageData = FileManager.SaveImage(imageData.WallId, imageData.FileContent);
            AddImageInSettings(newImageData);
        }

        public void ShareImage(Main.ImageData.ImageData imageData)
        {
            //todo: will be implemented in future
            Debug.Log($"Shared! {imageData.FileName}");
            // NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
            // NativeGallery.SaveImageToGallery(imageData.FileContent, "Album", imageData.FileName);
            //SendEmail.Send(imageData);
        }

        public void LoadSettings()
        {
            Settings = FileManager.LoadSettings();
        }

        private void AddImageInSettings(Main.ImageData.ImageData imageData)
        {
            Settings ??= new SettingsData();
            Settings.ImagesData ??= new List<Main.ImageData.ImageData>();
            Settings.ImagesData.Add(imageData);
        }
    }
}