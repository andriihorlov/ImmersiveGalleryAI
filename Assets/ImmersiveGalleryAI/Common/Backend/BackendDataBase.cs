using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using ImmersiveGalleryAI.Common.Settings;
using UnityEngine;
using Logger = ImmersiveGalleryAI.Common.Utilities.Logger;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendDataBase
    {
        private const string DataBaseUsers = "Users";
        private const string DataBaseLoginName = "login";
        private const string DataBaseEmailName = "email";

        private const string AdminEmailKey = "AdminEmail";
        private const string DefaultApiKey = "DefaultApi";
        private const string FreeImageCountKey = "FreeImageCount";

        private int _wallImageCount;
        private DatabaseReference _firebaseGetDatabaseReference;
        private DatabaseReference FirebaseGetDataBaseReference => _firebaseGetDatabaseReference ??= FirebaseDatabase.DefaultInstance.GetReference(DataBaseUsers);

        public void SetWallImageCount(int imagesCount)
        {
            _wallImageCount = imagesCount;
        }
        
        public async UniTask AddToDatabase(string login, string email, int defaultImagesLeft)
        {
            UserModel userModel = new UserModel
                {login = login, email = email, imageSettings = new ImageSetting[_wallImageCount], imagesLeft = defaultImagesLeft};
            DatabaseReference databaseReference = FirebaseGetDataBaseReference.Push();
            await databaseReference.SetRawJsonValueAsync(JsonUtility.ToJson(userModel)).ContinueWithOnMainThread(task => { Logger.WriteTask(task, "Add data to DB"); });
        }

        public async UniTask<bool> IsLoginExist(string login)
        {
            return await GetUser(login) != null;
        }
        
        public async UniTask<SettingsData> GetApplicationSettings()
        {
            SettingsData settingsData = new SettingsData();
            Task jsonAsync = FirebaseDatabase.DefaultInstance.GetReference(string.Empty)
                .GetValueAsync()
                .ContinueWith(dataSnapshot =>
                {
                    if (!dataSnapshot.IsCompletedSuccessfully)
                    {
                        return;
                    }

                    IEnumerable<DataSnapshot> resultChildren = dataSnapshot.Result.Children;
                    foreach (DataSnapshot snapshot in resultChildren)
                    {
                        if (snapshot == null)
                        {
                            continue;
                        }

                        switch (snapshot.Key)
                        {
                            case AdminEmailKey:
                                settingsData.AdminEmail = snapshot.Value.ToString();
                                break;
                            case DefaultApiKey:
                                settingsData.DefaultApi = snapshot.Value.ToString();
                                break;
                            case FreeImageCountKey:
                                settingsData.FreeImageCount = int.Parse(snapshot.Value.ToString());
                                break;
                        }
                    }
                });

            await jsonAsync;
            return settingsData;
        }

        public async UniTask<string> GetUserEmail(string login)
        {
            string userEmail = string.Empty;
            DataSnapshot user = await GetUser(login);
            if (user != null)
            {
                userEmail = GetSnapshotFieldData(user, DataBaseEmailName);
            }

            return userEmail;
        }

        private async UniTask<DataSnapshot> GetUser(string login)
        {
            DataSnapshot targetUserSnapshot = null;
            Task jsonAsync = FirebaseGetDataBaseReference.GetValueAsync().ContinueWith(dataSnapshot =>
            {
                if (!dataSnapshot.IsCompletedSuccessfully)
                {
                    return;
                }

                IEnumerable<DataSnapshot> resultChildren = dataSnapshot.Result.Children;
                Debug.Log($"Children count: {dataSnapshot.Result.ChildrenCount}");

                foreach (DataSnapshot snapshot in resultChildren)
                {
                    if (GetSnapshotFieldData(snapshot, DataBaseLoginName) != login)
                    {
                        continue;
                    }

                    targetUserSnapshot = snapshot;
                    break;
                }
            });

            await jsonAsync;
            return targetUserSnapshot;
        }

        private string GetSnapshotFieldData(DataSnapshot dataSnapshot, string targetChild)
        {
            return dataSnapshot?.Child(targetChild).Value.ToString();
        }
    }
}