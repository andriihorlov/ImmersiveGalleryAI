using System.Collections;
using ImmersiveGalleryAI.Common.Settings;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.Web
{
    public class WebData : MonoBehaviour
    {
        [SerializeField] private Texture2D[] _randomTextures;
        [SerializeField] private bool _isForcedDisableAi = false;

        [Inject] private IWebManager _webManager;
        [Inject] private ISettings _settings;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => _settings.IsSettingsReady);
            bool isAi = _settings.GetIsAiUse();
            if (_isForcedDisableAi)
            {
                isAi = false;
            }

            _webManager.Init(_randomTextures, isAi, _settings.GetCurrentApi());
        }
    }
}