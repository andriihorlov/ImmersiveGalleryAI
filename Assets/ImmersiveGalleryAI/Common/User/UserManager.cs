using ImmersiveGalleryAI.Common.Backend;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.User
{
    public class UserManager : IUser
    {
        private UserModel _currentUserModel;
        public bool IsGuest { get; set; }

        public void SetUserData(UserModel userModel)
        {
            _currentUserModel = userModel;
            Debug.Log($"Set User data {userModel.imageSettings.Length}");
        }

        public string GetCurrentUserLogin() => _currentUserModel?.login;
        public int GetUserCredits() => _currentUserModel.imagesLeft;
        public ImageSetting[] GetImageSettings() => _currentUserModel.imageSettings;
    }
}
