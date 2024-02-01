using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.Web
{
    public class WebData : MonoBehaviour
    {
        [SerializeField] private Texture2D[] _randomTextures;
        [SerializeField] private bool _isAi = false;

        [Inject] private IWebManager _webManager;

        private void Start()
        {
            _webManager.Init(_randomTextures, _isAi);
        }
    }
}
