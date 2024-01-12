using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace ImmersiveGalleryAI.Common.Backend
{
    public interface IBackend
    {
        Task<bool?> Registration(string login, string email, string password);
        void Login(string login, string password);
        UniTask<bool> RecoverPassword(string recoverEmail);
    }
}