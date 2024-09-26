using ImmersiveGalleryAI.Common.Backend;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.User
{
    public class UserManager : IUser
    {
        private UserModel _currentUserModel;
        public void SetUserData(UserModel userModel)
        {
            _currentUserModel = userModel;
            Debug.Log($"Set User data {userModel}");
        }

        public string GetCurrentUserLogin() => _currentUserModel?.login;
    }
}
