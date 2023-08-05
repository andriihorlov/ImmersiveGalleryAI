using Zenject;

namespace ImmersiveGalleryAI.Data
{
    public class DataManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IDataManager>().To<DataManager>().AsSingle();
        }
    }
}