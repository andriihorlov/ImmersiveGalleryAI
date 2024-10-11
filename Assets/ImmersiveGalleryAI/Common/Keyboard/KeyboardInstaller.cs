using VRUiKits.Utils;
using Zenject;

namespace ImmersiveGalleryAI.Common.Keyboard
{
    public class KeyBoardInstaller : MonoInstaller<KeyBoardInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IKeyboard>().To<KeyboardManager>().AsSingle();
        }
    }
}