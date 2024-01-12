﻿using System.Threading.Tasks;
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

        public void Login(string login, string password)
        {
            TryLogin(login, password);
        }

        public async UniTask<bool> RecoverPassword(string recoverEmail)
        {
            return await _backendAuth.RecoverPassword(recoverEmail);
        }

        private async UniTask<bool> IsLoginExist(string login)
        {
            bool isLoginExistTask = await _backendDataBase.IsLoginExist(login);
            Logger.WriteLog("Is LoginExist?", isLoginExistTask);
            return isLoginExistTask;
        }

        private async void TryLogin(string login, string password)
        {
            string userEmail = await _backendDataBase.GetUserEmail(login);

            if (string.IsNullOrEmpty(userEmail))
            {
                Logger.WriteLog("Can't logged!", false);
                return;
            }

            bool isLoginSucceed = await _backendAuth.Login(userEmail, password);
            if (isLoginSucceed)
            {
                Logger.WriteLog("Logged In!");
            }
            else
            {
                Logger.WriteLog("Can't logged!", false);
            }
        }
    }
}