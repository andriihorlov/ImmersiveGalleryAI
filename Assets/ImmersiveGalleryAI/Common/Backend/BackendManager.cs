using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ImmersiveGalleryAI.Main.ImageData;
using UnityEngine;
using UnityEngine.Playables;
using Logger = ImmersiveGalleryAI.Common.Utilities.Logger;
using SettingsData = ImmersiveGalleryAI.Common.Settings.SettingsData;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendManager : IBackend
    {
        private readonly BackendAuth _backendAuth;
        private readonly BackendDataBase _backendDataBase;
        private readonly BackendStorage _backendStorage;

        private int _defaultImageLeft;
        private string _currentUserLogin;

        public BackendManager()
        {
            _backendAuth = new BackendAuth();
            _backendDataBase = new BackendDataBase();
            _backendStorage = new BackendStorage();
        }

        public void SetWallImagesCount(int imageCount, int defaultImagesLeft)
        {
            _backendDataBase.SetWallImageCount(imageCount);
            _defaultImageLeft = defaultImagesLeft;
        }

        public async Task<bool> Registration(string login, string email, string password)
        {
            bool isPossibleContinue = await IsLoginExist(login);

            if (isPossibleContinue)
            {
                Debug.LogError($"User with this login already exist");
                return false;
            }

            bool isRegistrationSucceed = await _backendAuth.Registration(email, password);
            if (isRegistrationSucceed)
            {
                await _backendDataBase.AddToDatabase(login, email, _defaultImageLeft);
                _currentUserLogin = login;
            }

            return isRegistrationSucceed;
        }

        public async UniTask<bool> Login(string login, string password)
        {
            bool isLogged = await TryLogin(login, password);
            if (isLogged)
            {
                _backendStorage.Init(login);
                _currentUserLogin = login;
            }
            return isLogged;
        }

        public async UniTask<bool> RecoverPassword(string recoverEmail)
        {
            return await _backendAuth.RecoverPassword(recoverEmail);
        }

        public async UniTask<bool> UploadImageData(ImageData imageData)
        {
            string storagePath = await _backendStorage.UploadImage(imageData.WallId, imageData.FileContent);
            await _backendDataBase.UploadImageData(imageData, _currentUserLogin, storagePath);
            return !string.IsNullOrEmpty(storagePath);
        }

        public async UniTask<byte[]> DownloadImage(string imagePath)
        {
            return await _backendStorage.DownloadImage(imagePath);
        }

        public async UniTask<SettingsData> GetApplicationSettings()
        {
            return await _backendDataBase.GetApplicationSettings();
        }

        public async UniTask<UserModel> GetUserModel(string userName)
        {
            return await _backendDataBase.GetUserModel(userName);
        }

        public async UniTask GuestEnter()
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            string guestLogin = "Guest" + deviceId;
            bool isDeviceIdExist = await _backendDataBase.IsLoginExist(guestLogin);

            if (isDeviceIdExist)
            {
                return;
            }

            string tempEmail = deviceId.Substring(0, 5) + "@guest.com";
            await _backendDataBase.AddToDatabase(guestLogin, tempEmail, _defaultImageLeft);
            _currentUserLogin = "Guest" + guestLogin;
            Debug.Log($"Guest entered! {guestLogin} : {tempEmail}");
        }

        private async UniTask<bool> IsLoginExist(string login)
        {
            bool isLoginExistTask = await _backendDataBase.IsLoginExist(login);
            Logger.WriteLog("Is LoginExist?", isLoginExistTask);
            return isLoginExistTask;
        }

        private async UniTask<bool> TryLogin(string login, string password)
        {
            string userEmail = await _backendDataBase.GetUserEmail(login);

            if (string.IsNullOrEmpty(userEmail))
            {
                Logger.WriteLog("Can't find user email in database!", false);
                return false;
            }

            bool isLoginSucceed = await _backendAuth.Login(userEmail, password);
            if (isLoginSucceed)
            {
                Logger.WriteLog("Logged In!");
                return true;
            }

            Logger.WriteLog("Can't logged!", false);
            return false;
        }
    }
}