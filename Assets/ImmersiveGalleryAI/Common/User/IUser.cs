using ImmersiveGalleryAI.Common.Backend;

namespace ImmersiveGalleryAI.Common.User
{
    public interface IUser
    {
        bool IsGuest { get; set; }
        void SetUserData(UserModel userModel);
        string GetCurrentUserLogin();
        int GetUserCredits();
        ImageSetting[] GetImageSettings();
    }
}
