namespace ImmersiveGalleryAI.Common.Settings
{
    public interface ISettings
    {
        bool IsSettingsReady { get; }
        
        void SetSettings(SettingsData settingsData);
        void UseOwnApi(string api);
        string GetCurrentApi();
        bool GetIsAiUse();
        string GetAdminEmail();
        int GetDefaultImageCount();
        int GetAiImageSize();
        int GetAiModel();
    }
}
