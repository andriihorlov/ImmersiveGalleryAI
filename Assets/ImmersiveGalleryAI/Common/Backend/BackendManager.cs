using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ImmersiveGalleryAI.Main.Data;
using UnityEngine;
using Logger = ImmersiveGalleryAI.Common.Utilities.Logger;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendManager : IBackend
    {
        private readonly BackendAuth _backendAuth;
        private readonly BackendDataBase _backendDataBase;
        private readonly BackendStorage _backendStorage;

        public BackendManager()
        {
            _backendAuth = new BackendAuth();
            _backendDataBase = new BackendDataBase();
            _backendStorage = new BackendStorage();
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
                await _backendDataBase.AddToDatabase(login, email);
            }

            return isRegistrationSucceed;
        }

        public async UniTask<bool> Login(string login, string password)
        {
            bool isLogged = await TryLogin(login, password);
            if (isLogged)
            {
                _backendStorage.Init(login);
            }

            return isLogged;
        }

        public async UniTask<bool> RecoverPassword(string recoverEmail)
        {
            return await _backendAuth.RecoverPassword(recoverEmail);
        }

        public async UniTask<bool> UploadImageData(ImageData imageData)
        {
            return await _backendStorage.UploadImage(imageData.WallId, imageData.FileContent);
        }

        public async UniTask<byte[]> DownloadImage(string userName, int wallId)
        {
            return await _backendStorage.DownloadImage(userName, wallId);
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
                Logger.WriteLog("Can't logged!", false);
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