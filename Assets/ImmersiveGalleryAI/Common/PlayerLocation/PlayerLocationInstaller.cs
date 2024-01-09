using ImmersiveGalleryAI.Common.Web;
using Zenject;

namespace ImmersiveGalleryAI.Common.User
{
    public class UserInstaller : MonoInstaller<WebInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IUser>().To<UserManager>().AsSingle();
        }
    }
}
