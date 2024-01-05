using Zenject;

namespace ImmersiveGalleryAI.Main.Data
{
    public class DataManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IDataManager>().To<DataManager>().AsSingle();
        }
    }
}