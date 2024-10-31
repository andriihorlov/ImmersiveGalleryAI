using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using ImmersiveGalleryAI.Common.Settings;
using ImmersiveGalleryAI.Main.ImageData;
using UnityEngine;
using Logger = ImmersiveGalleryAI.Common.Utilities.Logger;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendDataBase
    {
        private const string Settings = "Settings";
        private const string DataBaseUsers = "Users";
        private const string DataBaseLoginName = "login";
        private const string DataBaseEmailName = "email";
        private const string DataBaseImageLeftName = "imagesLeft";

        private const string DataBaseImageSettings = "imageSettings";
        private const string DataBaseImageWallId = "wallId";
        private const string DataBaseImageDescription = "description";
        private const string DataBaseImageImagePath = "imagePath";

        private const string AdminEmailKey = "AdminEmail";
        private const string DefaultApiKey = "DefaultApi";
        private const string FreeImageCountKey = "FreeImageCount";
        private const string IsTestCountKey = "IsTest";
        private const string SenderEmailLogin = "SenderEmailLogin";
        private const string SenderEmailPassword = "SenderEmailPassword";
        private const string SenderEmailProvider = "SenderEmailProvider";

        private int _wallImageCount;
        private SettingsData _settingsData;

        private DatabaseReference _firebaseGetDatabaseReference;
        private DatabaseReference FirebaseGetDataBaseReference => _firebaseGetDatabaseReference ??= FirebaseDatabase.DefaultInstance.RootReference.Child(DataBaseUsers);

        public void SetWallImageCount(int imagesCount)
        {
            Debug.Log($"Set wall image count");
            _wallImageCount = imagesCount;
        }

        public async UniTask AddToDatabase(string login, string email, int defaultImagesLeft)
        {
            UserModel userModel = new UserModel
            {
                login = login,
                email = email,
                imagesLeft = defaultImagesLeft,
            };

            await FirebaseGetDataBaseReference.Child(login.ToLower()).SetRawJsonValueAsync(JsonUtility.ToJson(userModel))
                .ContinueWithOnMainThread(task => { Logger.WriteTask(task, "Add data to DB"); });
        }

        public async UniTask<bool> IsLoginExist(string login) => await GetUser(login) != null;

        public async UniTask<SettingsData> GetApplicationSettings()
        {
            SettingsData settingsData = new SettingsData();
            Task jsonAsync = FirebaseDatabase.DefaultInstance.RootReference.Child(Settings)
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
                            case IsTestCountKey:
                                settingsData.IsTest = bool.Parse(snapshot.Value.ToString());
                                break;
                            case SenderEmailLogin:
                                settingsData.SenderEmailLogin = snapshot.Value.ToString();
                                break;
                            case SenderEmailPassword:
                                settingsData.SenderEmailPassword = snapshot.Value.ToString();
                                break;
                            case SenderEmailProvider:
                                settingsData.SenderEmailProvider = snapshot.Value.ToString();
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
                userEmail = GetSnapshotFieldString(user, DataBaseEmailName);
            }

            return userEmail;
        }
        
        public async UniTask<UserModel> GetUserModel(string userName)
        {
            DataSnapshot user = await GetUser(userName);
            if (user == null)
            {
                return null;
            }

            UserModel userModel = new UserModel
            {
                imagesLeft = GetSnapshotFieldInt(user, DataBaseImageLeftName),
                login = userName,
                email = GetSnapshotFieldString(user, DataBaseEmailName),
            };

            List<ImageSetting> imageSettings = new List<ImageSetting>();

            foreach (DataSnapshot imageSnapshot in user.Child(DataBaseImageSettings).Children)
            {
                Debug.Log($"Data snapshot: {imageSnapshot.Key}");

                ImageSetting imageSetting = new ImageSetting()
                {
                    wallId = GetSnapshotFieldInt(imageSnapshot, DataBaseImageWallId),
                    description = GetSnapshotFieldString(imageSnapshot, DataBaseImageDescription),
                    imagePath = GetSnapshotFieldString(imageSnapshot, DataBaseImageImagePath),
                };

                imageSettings.Add(imageSetting);
            }

            userModel.imageSettings = imageSettings.ToArray();
            return userModel;
        }

        public async Task UploadImageData(ImageData imageData, string currentUserLogin, string dataBasePath)
        {
            UserModel userModel = await GetUserModel(currentUserLogin);
            foreach (ImageSetting userImage in userModel.imageSettings)
            {
                if (userImage.wallId != imageData.WallId)
                {
                    continue;
                }

                userImage.description = imageData.Description;
                userImage.imagePath = dataBasePath;
                break;
            }

            ImageSetting imageSetting = new ImageSetting()
            {
                description = imageData.Description,
                imagePath = dataBasePath,
                wallId = imageData.WallId
            };

            await FirebaseGetDataBaseReference.Child(currentUserLogin.ToLower()).Child(DataBaseImageSettings).Child(imageData.WallId.ToString())
                .SetRawJsonValueAsync(JsonUtility.ToJson(imageSetting))
                .ContinueWithOnMainThread(task => { Logger.WriteTask(task, "Add data to DB"); });
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

                foreach (DataSnapshot snapshot in resultChildren)
                {
                    string snapshotLogin = GetSnapshotFieldString(snapshot, DataBaseLoginName);

                    if (!IsLoginEqualWithoutCase(snapshotLogin, login))
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

        private bool IsLoginEqualWithoutCase(string snapshotLogin, string currentLogin) => string.Equals(snapshotLogin.ToLower(), currentLogin.ToLower());

        private string GetSnapshotFieldString(DataSnapshot dataSnapshot, string targetChild)
        {
            return dataSnapshot?.Child(targetChild) == null ? null : dataSnapshot?.Child(targetChild).Value.ToString();
        }

        private int GetSnapshotFieldInt(DataSnapshot dataSnapshot, string targetChild)
        {
            bool isParsed = int.TryParse(dataSnapshot?.Child(targetChild).Value.ToString(), out int result);
            if (!isParsed)
            {
                Debug.LogError($"Can't parse to INT -> {targetChild}");
            }

            return result;
        }
    }
}