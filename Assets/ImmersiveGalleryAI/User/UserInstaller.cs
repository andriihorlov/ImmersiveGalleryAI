using ImmersiveGalleryAI.Web;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.User
{
    public class UserInstaller : MonoInstaller<WebInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log($"User binding installed");
            Container.Bind<IUser>().To<UserManager>().AsSingle();
        }
    }
}
