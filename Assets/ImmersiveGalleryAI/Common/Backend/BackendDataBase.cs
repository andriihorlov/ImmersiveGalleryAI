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
        private const string DataBaseCustomApiName = "customApi";
        private const string DataBaseImageLeftName = "imagesLeft";
        private const string DataBaseAiModel = "aiModel";
        private const string DataBaseAiImageSize = "aiImageSize";

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

        private string _customApi;
        private SettingsData _settingsData;

        private DatabaseReference _firebaseGetDatabaseReference;
        private DatabaseReference FirebaseGetDataBaseReference => _firebaseGetDatabaseReference ??= FirebaseDatabase.DefaultInstance.RootReference.Child(DataBaseUsers);

        public async UniTask AddToDatabase(string login, string email, int defaultImagesLeft)
        {
            UserModel userModel = new UserModel
            {
                login = login,
                email = email,
                imagesLeft = defaultImagesLeft,
                customApi = _settingsData.DefaultApi,
                aiImageSize = _settingsData.DefaultImageSize,
                aiModelNumber = _settingsData.DefaultImageModel
            };

            await FirebaseGetDataBaseReference.Child(login.ToLower()).SetRawJsonValueAsync(JsonUtility.ToJson(userModel))
                .ContinueWithOnMainThread(task => { Logger.WriteTask(task, "Add data to DB"); });
        }

        public async UniTask<bool> IsLoginExist(string login) => await GetUser(login) != null;

        public async UniTask<SettingsData> GetApplicationSettings()
        {
            Debug.Log($"Get application settings");
            _settingsData = new SettingsData();
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
                                _settingsData.AdminEmail = snapshot.Value.ToString();
                                break;
                            case DefaultApiKey:
                                _settingsData.DefaultApi = snapshot.Value.ToString();
                                break;
                            case FreeImageCountKey:
                                _settingsData.FreeImageCount = int.Parse(snapshot.Value.ToString());
                                break;
                            case IsTestCountKey:
                                _settingsData.IsTest = bool.Parse(snapshot.Value.ToString());
                                break;
                            case SenderEmailLogin:
                                _settingsData.SenderEmailLogin = snapshot.Value.ToString();
                                break;
                            case SenderEmailPassword:
                                _settingsData.SenderEmailPassword = snapshot.Value.ToString();
                                break;
                            case SenderEmailProvider:
                                _settingsData.SenderEmailProvider = snapshot.Value.ToString();
                                break;
                            case DataBaseAiModel:
                                _settingsData.DefaultImageModel = int.Parse(snapshot.Value.ToString());
                                break;
                            case DataBaseAiImageSize:
                                _settingsData.DefaultImageSize = int.Parse(snapshot.Value.ToString());
                                break;
                        }
                    }
                });

            await jsonAsync;
            return _settingsData;
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
                customApi = GetSnapshotFieldString(user, DataBaseCustomApiName),
                aiImageSize = GetSnapshotFieldInt(user, DataBaseAiImageSize),
                aiModelNumber = GetSnapshotFieldInt(user, DataBaseAiModel),
            };

            if (_settingsData != null)
            {
                if (userModel.aiModelNumber > 0)
                {
                    _settingsData.DefaultImageModel = userModel.aiModelNumber;
                    Debug.Log($"Model type Parsed");
                }

                if (!string.IsNullOrEmpty(userModel.customApi))
                {
                    _settingsData.DefaultApi = userModel.customApi;
                    Debug.Log($"Custom API Parsed");
                }

                if (userModel.aiImageSize > 0)
                {
                    _settingsData.DefaultImageSize = userModel.aiImageSize;
                    Debug.Log($"Image size Parsed");
                }
            }
            
            List<ImageSetting> imageSettings = new List<ImageSetting>();

            foreach (DataSnapshot imageSnapshot in user.Child(DataBaseImageSettings).Children)
            {
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

        public async void UpdateCreditsBalance(string currentUserLogin, int creditsBalance)
        {
            await FirebaseGetDataBaseReference.Child(currentUserLogin.ToLower())
                .Child(DataBaseImageLeftName)
                .SetValueAsync(creditsBalance)
                .ContinueWithOnMainThread(task => { Logger.WriteTask(task, "Credits updated"); });
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
            return dataSnapshot?.Child(targetChild) == null ? null : dataSnapshot?.Child(targetChild)?.Value?.ToString();
        }

        private int GetSnapshotFieldInt(DataSnapshot dataSnapshot, string targetChild)
        {
            bool isParsed = int.TryParse(dataSnapshot?.Child(targetChild)?.Value?.ToString(), out int result);
            if (!isParsed)
            {
                Debug.LogError($"Can't parse to INT -> {targetChild}");
            }

            return result;
        }
    }
}