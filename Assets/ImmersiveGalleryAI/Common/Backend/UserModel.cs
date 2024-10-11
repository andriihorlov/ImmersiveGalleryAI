using System;

namespace ImmersiveGalleryAI.Common.Backend
{
    [Serializable]
    public class AllUsersModel
    {
        public UserModel[] users;
    }

    [Serializable]
    public class UserModel
    {
        public string login;
        public string email;
        public int imagesLeft;
        public ImageSetting[] imageSettings;
    }

    [Serializable]
    public class ImageSetting
    {
        public int wallId;
        public string imagePath;
        public string description;
    }
}