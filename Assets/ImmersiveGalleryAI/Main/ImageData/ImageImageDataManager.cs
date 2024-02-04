using System.Collections.Generic;
using ImmersiveGalleryAI.Common.Utilities;
using UnityEngine;

namespace ImmersiveGalleryAI.Main.ImageData
{
    public class ImageImageDataManager : IImageDataManager
    {
        public AllImages Settings { get; private set; }

        public void SaveSettings()
        {
            FileManager.SaveImages(Settings);
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
            // NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
            // NativeGallery.SaveImageToGallery(imageData.FileContent, "Album", imageData.FileName);
            //SendEmail.Send(imageData);
        }

        public void LoadSettings()
        {
            Settings = FileManager.LoadAllImages();
        }

        private void AddImageInSettings(ImageData imageData)
        {
            Settings ??= new AllImages();
            Settings.ImagesData ??= new List<ImageData>();
            Settings.ImagesData.Add(imageData);
        }
    }
}