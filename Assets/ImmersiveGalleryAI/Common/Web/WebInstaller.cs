using Zenject;

namespace ImmersiveGalleryAI.Common.Web
{
    public class WebInstaller : MonoInstaller<WebInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IWebManager>().To<WebManager>().AsSingle();
        }
    }
}