using Zenject;

namespace ImmersiveGalleryAI.Main.Data
{
    public class ImageDataManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IImageDataManager>().To<ImageImageDataManager>().AsSingle();
        }
    }
}