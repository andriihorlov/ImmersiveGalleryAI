using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendManager : IBackend
    {
        private FirebaseAuth _firebaseAuth;
        private FirebaseAuth FirebaseAuth => _firebaseAuth ??= FirebaseAuth.DefaultInstance;
        
        public void Registration(string email, string password)
        {
            Task<AuthResult> registerTask = FirebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
            TryToRegistration(registerTask);
        }

        public void Login(string login, string password)
        {
            
        }

        private async void TryToRegistration(Task<AuthResult> registerTask)
        {
            await UniTask.Yield();
            await UniTask.WaitUntil(() => registerTask.IsCompleted);

            if (registerTask.Exception != null)
            {
                Debug.LogWarning($"Registration failed, because: {registerTask.Exception.Message}");
            }
            else
            {
                Debug.Log($"Registration succeed! {registerTask.Result.Credential} | {registerTask.Result.User.Email} | {registerTask.Result.User.DisplayName}");
            }
        }
    }
}