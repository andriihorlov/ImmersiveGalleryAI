using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.User
{
    public class UserInstaller : MonoInstaller<UserInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log($"User Installer binding");
            Container.Bind<IUser>().To<UserManager>().AsSingle();
        }
    }
}
