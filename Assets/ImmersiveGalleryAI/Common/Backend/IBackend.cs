using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ImmersiveGalleryAI.Main.Data;

namespace ImmersiveGalleryAI.Common.Backend
{
    public interface IBackend
    {
        Task<bool> Registration(string login, string email, string password);
        UniTask<bool> Login(string login, string password);
        UniTask<bool> RecoverPassword(string recoverEmail);

        UniTask<bool> UploadImageData(ImageData imageData);
        UniTask<byte[]> DownloadImage(string userName, int wallId);
    }
}