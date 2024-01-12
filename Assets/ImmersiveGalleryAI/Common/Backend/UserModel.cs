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

        public string[] imagesPath;
    }
}