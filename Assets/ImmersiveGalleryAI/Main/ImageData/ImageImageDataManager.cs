using System;
using System.Collections.Generic;
using ImmersiveGalleryAI.Common.Utilities;
using UnityEngine;

namespace ImmersiveGalleryAI.Main.ImageData
{
    public class ImageImageDataManager : IImageDataManager
    {
        private IImageDataManager _imageDataManagerImplementation;
        public event Action UpdatePreviousImagesEvent;
        public AllImages Settings { get; private set; }

        public void SaveSettings(string owner)
        {
            FileManager.SaveImages(Settings, owner);
        }

        public void DeleteImage(ImageData imageData)
        {
            if (imageData == null)
            {
                return;
            }
            
            Settings.ImagesData.Remove(imageData);
            FileManager.DeleteImage(imageData.FilePath);
        }

        public void SaveImage(ImageData imageData)
        {
            ImageData newImageData = FileManager.SaveNewImage(imageData, Settings.Owner);
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

        public void UpdatePreviousImages()
        {
            UpdatePreviousImagesEvent?.Invoke();
        }

        public void LoadSettings(string owner)
        {
            Settings = FileManager.LoadAllImages(owner);
            Settings.Owner = owner;
        }

        private void AddImageInSettings(ImageData imageData)
        {
            Settings ??= new AllImages();
            Settings.ImagesData ??= new List<ImageData>();
            Settings.ImagesData.Add(imageData);
        }
    }
}