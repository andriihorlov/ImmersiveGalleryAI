using ImmersiveGalleryAI.Common.Backend;

namespace ImmersiveGalleryAI.Common.User
{
    public interface IUser
    {
        void SetUserData(UserModel userModel);
        string GetCurrentUserLogin();
    }
}
