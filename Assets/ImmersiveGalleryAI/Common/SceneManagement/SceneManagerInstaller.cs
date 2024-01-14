using Zenject;

namespace ImmersiveGalleryAI.Common.SceneManagement
{
    public class SceneManagerInstaller : MonoInstaller<SceneManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ISceneManager>().To<SceneManager>().AsSingle();
        }
    }
}