using System;

namespace ImmersiveGalleryAI.Common.Settings
{
    [Serializable]
    public class SettingsData
    {
        public string AdminEmail;
        public string DefaultApi;
        public int FreeImageCount;
        public bool IsTest;
        public string SenderEmailLogin;
        public string SenderEmailPassword;
        public string SenderEmailProvider;
    }
}