using System;

namespace ImmersiveGalleryAI.Main.ImageData
{
    public interface IImageDataManager
    {
        event Action UpdatePreviousImagesEvent;
        
        AllImages Settings { get; }
        void SaveSettings(string owner);
        void LoadSettings(string owner);
        void DeleteImage(ImageData imageData);
        void SaveImage(ImageData imageData);
        void ShareImage(ImageData imageData);
        void UpdatePreviousImages();
    }
}