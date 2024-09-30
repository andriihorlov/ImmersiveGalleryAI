using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Extensions;
using Firebase.Storage;
using UnityEngine;
using Logger = ImmersiveGalleryAI.Common.Utilities.Logger;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendStorage
    {
        private const long MaxAllowedImageSize = 1024 * 1024;
        private const string StorageUrl = "gs://immersivegalleryai.appspot.com";

        private FirebaseStorage _firebaseStorageReference;
        private FirebaseStorage FirebaseStorageReference => _firebaseStorageReference ??= FirebaseStorage.DefaultInstance;
        
        private string _currentUserLogin;

        public void Init(string userLogin)
        {
            _currentUserLogin = userLogin;
        }

        public async UniTask<byte[]> DownloadImage(string userName, int wallId)
        {
            StorageReference storageRef = FirebaseStorageReference.GetReferenceFromUrl(GetImagePath(userName, wallId));
            byte[] bytes = null;

            await storageRef.GetBytesAsync(MaxAllowedImageSize).ContinueWithOnMainThread(task =>
            {
                Logger.WriteTask(task, nameof(DownloadImage));
                if (task.IsCompletedSuccessfully)
                {
                    bytes = task.Result;
                }
            });
            return bytes;
        }

        public async UniTask<bool> UploadImage(int wallId, byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                Logger.WriteLog("Image byte array is null or empty.", false);
                return false;
            }

            if (bytes.Length > MaxAllowedImageSize)
            {
                Logger.WriteLog("Image size exceeds the maximum allowed size of 1 MB.", false);
                return false;
            }

            StorageReference storageRef = FirebaseStorageReference.GetReferenceFromUrl(StorageUrl);
            StorageReference wallImageRef = storageRef.Child(GetImagePath(_currentUserLogin, wallId));

            Debug.Log($"Storage: {storageRef} | Image path: {wallImageRef.Path}");

            if (string.IsNullOrEmpty(wallImageRef.Path))
            {
                return false;
            }
            
            
            Task uploadImage = wallImageRef.PutBytesAsync(bytes).ContinueWithOnMainThread(task =>
            {
                Logger.WriteTask(task, "Upload file");
                if (!task.IsCompletedSuccessfully)
                {
                    return;
                }

                StorageMetadata metadata = task.Result;
                string md5Hash = metadata.Md5Hash;
                Debug.Log("Finished uploading...");
                Debug.Log("md5 hash = " + md5Hash);
            });

            await uploadImage;
            
            return uploadImage.IsCompletedSuccessfully;
        }


        private string GetImagePath(string userName, int index) => $"images/users/{userName}/{userName}_{index}.jpg";
    }
}