using System;

namespace ImmersiveGalleryAI.Common.AudioSystem
{
    public class AudioManager : IAudioSystem
    {
        public event Action PlayAudioClick;
        public event Action PlayAudioMusic;

        public void PlayMusic()
        {
            PlayAudioMusic?.Invoke();
        }

        public void PlayClickSfx()
        {
            PlayAudioClick?.Invoke();
        }
    }
}