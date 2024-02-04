namespace ImmersiveGalleryAI.Common.Settings
{
    public class SettingsManager : ISettings
    {
        private SettingsData _settingsData;

        public void SetSettings(SettingsData settingsData)
        {
            _settingsData = settingsData;
        }

        public void UseOwnApi(string api)
        {
            _settingsData.DefaultApi = api;
        }

        public string GetCurrentApi() => _settingsData?.DefaultApi;
        public string GetAdminEmail() => _settingsData?.AdminEmail;
        public int GetDefaultImageCount() => _settingsData?.FreeImageCount ?? -1;
    }
}