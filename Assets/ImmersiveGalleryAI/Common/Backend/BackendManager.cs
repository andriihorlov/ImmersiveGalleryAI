using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Logger = ImmersiveGalleryAI.Common.Utilities.Logger;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendManager : IBackend
    {
        private readonly BackendAuth _backendAuth;
        private readonly BackendDataBase _backendDataBase;

        public BackendManager()
        {
            _backendAuth = new BackendAuth();
            _backendDataBase = new BackendDataBase();
        }

        public async Task<bool?> Registration(string login, string email, string password)
        {
            bool isPossibleContinue = false;

            await IsLoginExist(login).ContinueWith(isLoginExist => { isPossibleContinue = isLoginExist; });

            if (isPossibleContinue)
            {
                Debug.LogError($"User with this login already exist");
                return null;
            }

            bool isRegistrationSucceed = await _backendAuth.Registration(email, password);
            if (isRegistrationSucceed)
            {
                await _backendDataBase.AddToDatabase(login, email);
            }

            return isRegistrationSucceed;
        }

        private async UniTask<bool> IsLoginExist(string login)
        {
            UniTask<bool> isLoginExistTask = _backendDataBase.IsLoginExist(login);
            bool isExist = false;
            await isLoginExistTask.ContinueWith(isLoginExistResult => { isExist = isLoginExistResult; });
            Logger.WriteTask(isLoginExistTask, "Is Login exist");
            
            return isExist;
        }

        public void Login(string login, string password) { }

        public async UniTask<bool> RecoverPassword(string recoverEmail)
        {
            return await _backendAuth.RecoverPassword(recoverEmail);
        }
    }
}