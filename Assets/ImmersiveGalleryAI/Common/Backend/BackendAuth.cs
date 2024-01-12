using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Auth;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendAuth
    {
        private FirebaseAuth _firebaseAuth;
        private FirebaseAuth FirebaseAuth => _firebaseAuth ??= FirebaseAuth.DefaultInstance;

        public async UniTask<bool> Registration(string email, string password)
        {
            Task<AuthResult> registerTask = FirebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
            return await TryToRegistration(registerTask);
        }

        public async UniTask<bool> RecoverPassword(string recoverEmail)
        {
             Task resetEmailTask = FirebaseAuth.SendPasswordResetEmailAsync(recoverEmail);
             await resetEmailTask;
             return resetEmailTask.IsCompletedSuccessfully;
        }

        private async UniTask<bool> TryToRegistration(Task<AuthResult> registerTask)
        {
            await UniTask.Yield();
            await UniTask.WaitUntil(() => registerTask.IsCompleted);
            return registerTask.IsCompletedSuccessfully;
        }
    }
}