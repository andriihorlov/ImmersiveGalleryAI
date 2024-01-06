using Zenject;

namespace ImmersiveGalleryAI.Common.Backend
{
    public class BackendInstaller : MonoInstaller<BackendInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IBackend>().To<BackendManager>().AsSingle();
        }
    }
}