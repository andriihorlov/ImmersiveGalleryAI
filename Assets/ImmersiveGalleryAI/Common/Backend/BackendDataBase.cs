using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using Logger = ImmersiveGalleryAI.Common.Utilities.Logger;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendDataBase
    {
        private const string DataBaseUsers = "Users";

        private DatabaseReference _firebaseGetDatabaseReference;
        private DatabaseReference FirebaseGetDataBaseReference => _firebaseGetDatabaseReference ??= FirebaseDatabase.DefaultInstance.GetReference(DataBaseUsers);
        
        public async UniTask AddToDatabase(string login, string email)
        {
            UserModel userModel = new UserModel {login = login, email = email, imagesPath = new string[0]};
            DatabaseReference databaseReference = FirebaseGetDataBaseReference.Push();
            await databaseReference.SetRawJsonValueAsync(JsonUtility.ToJson(userModel)).ContinueWithOnMainThread(task => { Logger.WriteTask(task, "Add data to DB"); });
        }

        /// <summary>
        /// have to be implemented
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> IsLoginExist(string login)
        {
            bool isLoginExist = false;

            Task jsonAsync = FirebaseGetDataBaseReference.GetValueAsync().ContinueWith(dataSnapshot =>
            {
                if (!dataSnapshot.IsCompletedSuccessfully)
                {
                    return;
                }

                IEnumerable<DataSnapshot> resultChildren = dataSnapshot.Result.Children;
                Debug.Log($"Kids count: {dataSnapshot.Result.ChildrenCount}");

                foreach (DataSnapshot snapshot in resultChildren)
                {
                    isLoginExist = snapshot.Child("login").Value.ToString() == login;
                    if (isLoginExist)
                    {
                        break;
                    }
                }
            });

            await jsonAsync;
            return isLoginExist;
        }
    }
}