using Zenject;

namespace ImmersiveGalleryAI.Common.Experience
{
    public class ExperienceInstaller : MonoInstaller<ExperienceInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IExperienceManager>().To<ExperienceManager>().AsSingle();
        }
    }
}