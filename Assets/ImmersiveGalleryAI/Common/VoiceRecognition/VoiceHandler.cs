using Oculus.Voice;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.VoiceRecognition
{
    public class VoiceHandler : MonoBehaviour
    {
        [SerializeField] private AppVoiceExperience _appVoiceExperience;
        
        [Inject] private IVoiceHandler _voiceHandler;
        
        private void Awake()
        {
            _voiceHandler.Init(_appVoiceExperience);
        }

        private void OnEnable()
        {
            _voiceHandler.ToggleListeners(true);
        }

        private void OnDisable()
        {
            _voiceHandler.ToggleListeners(false);
        }
    }
}
