namespace ImmersiveGalleryAI.Main.Data
{
    public interface IDataManager
    {
        SettingsData Settings { get; }
        void SaveSettings();
        void LoadSettings();
        void DeleteImage(ImageData imageData);
        void SaveImage(ImageData imageData);
        void ShareImage(ImageData imageData);
    }
}