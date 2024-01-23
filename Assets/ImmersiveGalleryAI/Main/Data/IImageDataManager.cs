namespace ImmersiveGalleryAI.Main.Data
{
    public interface IImageDataManager
    {
        SettingsData Settings { get; }
        void SaveSettings();
        void LoadSettings();
        void DeleteImage(ImageData imageData);
        void SaveImage(ImageData imageData);
        void ShareImage(ImageData imageData);
    }
}