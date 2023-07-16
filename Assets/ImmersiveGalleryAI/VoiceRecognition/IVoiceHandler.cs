using System;
using Oculus.Voice;

namespace ImmersiveGalleryAI.VoiceRecognition
{
    public interface IVoiceHandler
    {
        event Action<string> TranscriptionDoneEvent;
        event Action StoppedListeningEvent;
        void Activate();
        public void Init(AppVoiceExperience voiceExperience);
        public void ToggleListeners(bool isActive);
    }
}