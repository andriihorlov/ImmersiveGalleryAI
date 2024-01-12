using ImmersiveGalleryAI.Common.Web;
using Zenject;

namespace ImmersiveGalleryAI.Common.PlayerLocation
{
    public class PlayerLocationInstaller : MonoInstaller<WebInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerLocation>().To<PlayerLocationManager>().AsSingle();
        }
    }
}
