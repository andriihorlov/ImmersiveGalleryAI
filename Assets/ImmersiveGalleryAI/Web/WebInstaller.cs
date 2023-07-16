using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Web
{
    public class WebInstaller : MonoInstaller<WebInstaller>
    {
        public override void InstallBindings()
        {
            Debug.Log($"Bindings installed");
            Container.Bind<IWebManager>().To<WebManager>().AsSingle();
        }
    }
}