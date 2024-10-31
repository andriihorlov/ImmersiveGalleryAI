using System.Threading.Tasks;
using ImmersiveGalleryAI.Common.AudioSystem;
using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.User;
using ImmersiveGalleryAI.Common.Utilities;
using ImmersiveGalleryAI.Main.Credits;
using ImmersiveGalleryAI.Main.ImageData;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.Settings
{
    public class ApplicationSettings : MonoBehaviour
    {
        [Inject] private IBackend _backend;
        [Inject] private ISettings _settings;
        [Inject] private ICredits _credits;
        [Inject] private IUser _user;
        [Inject] private IAudioSystem _audioSystem;
        [Inject] private IImageDataManager _imageDataManager;

        private void Start()
        {
            InitSettings();
            _audioSystem.PlayMusic();
        }

        private void OnEnable()
        {
            _credits.RequestUpgradeBalanceEvent += RequestCreditUpgradeEventHandler;
        }

        private void OnDisable()
        {
            _credits.RequestUpgradeBalanceEvent -= RequestCreditUpgradeEventHandler;
        }

        private async void InitSettings()
        {
            await FillImagesFromBackend();
            _imageDataManager.UpdatePreviousImages();

            string localOpenAiApi = FileManager.LoadLocalOpenAiApi();
            if (string.IsNullOrEmpty(localOpenAiApi))
            {
                Debug.Log($"Local AI settings not exist");
                SetCredits();
                _credits.SetCreditType(isOwn: false);
                return;
            }

            SetCredits();
            _settings.UseOwnApi(localOpenAiApi);
            _credits.SetCreditType(isOwn: true);
        }

        private async Task FillImagesFromBackend()
        {
            ImageSetting[] backendImages = _user.GetImageSettings();
            _imageDataManager.LoadSettings(_user.GetCurrentUserLogin());
            AllImages localImages = _imageDataManager.Settings;

            foreach (ImageSetting backendImage in backendImages)
            {
                if (localImages == null || localImages.ImagesData?.Count < 1)
                {
                    await DownloadAndSaveImage(backendImage);
                    continue;
                }

                bool isImageFound = false;

                foreach (ImageData imageData in localImages.ImagesData)
                {
                    if (imageData.WallId == backendImage.wallId && imageData.Description == backendImage.description)
                    {
                        isImageFound = true;
                        break;
                    }
                }

                if (isImageFound)
                {
                    continue;
                }
                
                await DownloadAndSaveImage(backendImage);
            }
            
            _imageDataManager.UpdatePreviousImages();
        }

        private async Task DownloadAndSaveImage(ImageSetting backendImage)
        {
            byte[] image = await _backend.DownloadImage(backendImage.imagePath);
            if (image == null)
            {
                Debug.Log($"Can't download image for {backendImage.wallId} wall.");
                return;
            }

            _imageDataManager.SaveImage(GetNewImageData(backendImage.wallId, backendImage.description, image));
        }

        private ImageData GetNewImageData(int wallId, string description, byte[] fileContent)
        {
            return new ImageData()
            {
                WallId = wallId,
                Description = description,
                FileContent = fileContent,
            };
        }

        private void RequestCreditUpgradeEventHandler()
        {
            _backend.SendRequestEmailFrom(_user.GetUserEmail());
        }

        private void SetCredits()
        {
            _credits.SetCreditsBalance(_user.GetUserCredits());
        }
    }
}