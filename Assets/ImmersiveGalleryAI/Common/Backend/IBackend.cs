using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ImmersiveGalleryAI.Main.ImageData;
using SettingsData = ImmersiveGalleryAI.Common.Settings.SettingsData;

namespace ImmersiveGalleryAI.Common.Backend
{
    public interface IBackend
    {
        Task<bool> Registration(string login, string email, string password);
        UniTask<bool> Login(string login, string password);
        UniTask<bool> RecoverPassword(string recoverEmail);
        UniTask GuestEnter();

        UniTask<bool> UploadImageData(ImageData imageData);
        UniTask<byte[]> DownloadImage(string imagePath);

        UniTask<SettingsData> GetApplicationSettings();
        UniTask<UserModel> GetUserModel(string userName);
        void SendRequestEmailFrom(string userEmail);
        void UpdateCreditsBalance(int credits);
    }
}