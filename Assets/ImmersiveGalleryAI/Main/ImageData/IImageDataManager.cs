using System;

namespace ImmersiveGalleryAI.Main.ImageData
{
    public interface IImageDataManager
    {
        event Action UpdatePreviousImagesEvent;
        
        AllImages Settings { get; }
        void SaveSettings();
        void LoadSettings();
        void DeleteImage(ImageData imageData);
        void SaveImage(ImageData imageData);
        void ShareImage(ImageData imageData);
        void UpdatePreviousImages();
    }
}