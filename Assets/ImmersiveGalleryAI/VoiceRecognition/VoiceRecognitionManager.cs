using System;
using Oculus.Voice;

namespace ImmersiveGalleryAI.VoiceRecognition
{
    public class VoiceRecognitionManager : IVoiceHandler
    {
        public event Action<string> TranscriptionDoneEvent;
        public event Action StoppedListeningEvent;

        private AppVoiceExperience _appVoiceExperience;
        
        public void Init(AppVoiceExperience voiceExperience)
        {
            _appVoiceExperience = voiceExperience;
        }

        public void Activate()
        {
            _appVoiceExperience.Activate();
        }

        public void ToggleListeners(bool isActive)
        {
            if (isActive)
            {
                _appVoiceExperience.events.OnPartialTranscription.AddListener(OnRequestTranscript);
                _appVoiceExperience.events.OnFullTranscription.AddListener(OnRequestTranscript);
                _appVoiceExperience.events.OnStoppedListeningDueToDeactivation.AddListener(OnStoppedListeningDueToDeactivation);   
            }
            else
            {
                _appVoiceExperience.events.OnPartialTranscription.RemoveListener(OnRequestTranscript);
                _appVoiceExperience.events.OnFullTranscription.RemoveListener(OnRequestTranscript);
                _appVoiceExperience.events.OnStoppedListeningDueToDeactivation.RemoveListener(OnStoppedListeningDueToDeactivation);
            }
        }
        
        private void OnRequestTranscript(string transcript)
        {
            TranscriptionDoneEvent?.Invoke(transcript);
        }

        private void OnStoppedListeningDueToDeactivation()
        {
            StoppedListeningEvent?.Invoke();
        }
    }
}
