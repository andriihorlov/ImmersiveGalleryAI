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

            OpenAiSettings openAiSettings = new OpenAiSettings()
            {
                Api = _settings.GetCurrentApi(),
                ImageSize = _settings.GetAiImageSize(),
                Model = _settings.GetAiModel()
            };
            
            Debug.Log($"Init ai settings: Model: {openAiSettings.Model}. Size: {openAiSettings.ImageSize}");
            
            _webManager.Init(_randomTextures, isAi, openAiSettings);
        }
    }
}