using Zenject;

namespace ImmersiveGalleryAI.VoiceRecognition
{
    public class VoiceRecognitionInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IVoiceHandler>().To<VoiceRecognitionManager>().AsSingle();
        }
    }
}