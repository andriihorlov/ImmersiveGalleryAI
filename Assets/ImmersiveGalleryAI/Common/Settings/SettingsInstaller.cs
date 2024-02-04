using Zenject;

namespace ImmersiveGalleryAI.Common.Settings
{
    public class SettingsInstaller : MonoInstaller<SettingsInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISettings>().To<SettingsManager>().AsSingle();
        }
    }
}