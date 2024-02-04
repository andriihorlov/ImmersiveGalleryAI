namespace ImmersiveGalleryAI.Common.Settings
{
    public interface ISettings
    {
        void SetSettings(SettingsData settingsData);
        void UseOwnApi(string api);
        string GetCurrentApi();
        string GetAdminEmail();
        int GetDefaultImageCount();
    }
}
