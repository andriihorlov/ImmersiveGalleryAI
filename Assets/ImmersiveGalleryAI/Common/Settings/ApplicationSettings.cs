using Cysharp.Threading.Tasks;
using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.User;
using ImmersiveGalleryAI.Common.Utilities;
using ImmersiveGalleryAI.Main.Credits;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.Settings
{
    public class ApplicationSettings : MonoBehaviour
    {
        [Tooltip("Count of image places")]
        [SerializeField]
        public int _wallImages = 11;

        [Inject] private IBackend _backend;
        [Inject] private ISettings _settings;
        [Inject] private ICredits _credits;
        [Inject] private IUser _user;

        private void Start()
        {
            InitSettings();
        }

        private async void InitSettings()
        {
            UniTask<SettingsData> settings = _backend.GetApplicationSettings();
            SettingsData settingsData = await settings;
            _settings.SetSettings(settingsData);
            
            string localOpenAiApi = FileManager.LoadLocalOpenAiApi();
            if (string.IsNullOrEmpty(localOpenAiApi))
            {
                Debug.Log($"Local settings not exist");
                _backend.SetWallImagesCount(_wallImages, _settings.GetDefaultImageCount());
                _credits.SetCreditType(isOwn: false);
                return;
            }

            _settings.UseOwnApi(localOpenAiApi);
            _credits.SetCreditType(isOwn: true);
        }

        [ContextMenu("GetUser login")]
        private void GetUserLogin()
        {
            Debug.Log($"Login: {_user.GetCurrentUserLogin()}");
        }
    }
}