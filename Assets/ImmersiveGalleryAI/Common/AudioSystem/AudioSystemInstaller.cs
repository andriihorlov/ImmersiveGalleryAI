using Zenject;

namespace ImmersiveGalleryAI.Common.AudioSystem
{
    public class AudioSystemInstaller : MonoInstaller<AudioSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IAudioSystem>().To<AudioManager>().AsSingle();
        }
    }
}
