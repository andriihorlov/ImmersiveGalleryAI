namespace ImmersiveGalleryAI.Common.Settings
{
    public class SettingsManager : ISettings
    {
        private SettingsData _settingsData;

        public bool IsSettingsReady { get; private set; }
        public void SetSettings(SettingsData settingsData)
        {
            _settingsData = settingsData;
            IsSettingsReady = true;
        }

        public void UseOwnApi(string api) => _settingsData.DefaultApi = api;
        public string GetCurrentApi() => _settingsData.DefaultApi;
        public bool GetIsAiUse() => !_settingsData.IsTest;
        public string GetAdminEmail() => _settingsData.AdminEmail;
        public int GetDefaultImageCount() => _settingsData?.FreeImageCount ?? -1;
        
    }
}