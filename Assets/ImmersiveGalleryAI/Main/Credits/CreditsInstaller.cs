using Zenject;

namespace ImmersiveGalleryAI.Main.Credits
{
    public class CreditsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ICredits>().To<CreditsManager>().AsSingle();
        }
    }
}