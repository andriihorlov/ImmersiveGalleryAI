using Zenject;

namespace ImmersiveGalleryAI.Main.ImageData
{
    public class ImageDataManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IImageDataManager>().To<ImageImageDataManager>().AsSingle();
        }
    }
}