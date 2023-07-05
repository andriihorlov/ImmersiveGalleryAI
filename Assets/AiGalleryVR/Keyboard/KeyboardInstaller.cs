using UnityEngine;
using VRUiKits.Utils;
using Zenject;

namespace AiGalleryVR.Keyboard
{
    public class KeyBoardInstaller : MonoInstaller<KeyBoardInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IKeyboard>().To<KeyboardManager>().AsSingle();
            Debug.Log($"Keyboard binds installed");
        }
    }
}