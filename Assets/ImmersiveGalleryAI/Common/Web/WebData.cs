using System.Collections;
using ImmersiveGalleryAI.Common.Settings;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.Web
{
    public class WebData : MonoBehaviour
    {
        [SerializeField] private Texture2D[] _randomTextures;
        [SerializeField] private bool _isAi = false;

        [Inject] private IWebManager _webManager;
        [Inject] private ISettings _settings;

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            _webManager.Init(_randomTextures, _isAi, _settings.GetCurrentApi());
        }
    }
}