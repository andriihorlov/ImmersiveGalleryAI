using ImmersiveGalleryAI.Common.Backend;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.User
{
    public class UserManager : IUser
    {
        private UserModel _currentUserModel;
        public bool IsGuest { get; set; }

        public void SetUserData(UserModel userModel) => _currentUserModel = userModel;
        public string GetCurrentUserLogin() => _currentUserModel?.login;
        public string GetUserEmail() => _currentUserModel?.email;
        public int GetUserCredits() => _currentUserModel.imagesLeft;
        public ImageSetting[] GetImageSettings() => _currentUserModel.imageSettings;
    }
}
