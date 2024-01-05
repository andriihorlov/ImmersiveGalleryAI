using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.Web
{
    public class WebData : MonoBehaviour
    {
        [SerializeField] private Sprite[] _randomSprites;
        [SerializeField] private bool _isAi = false;

        [Inject] private IWebManager _webManager;

        private void Start()
        {
            _webManager.Init(_randomSprites, _isAi);
        }
    }
}
