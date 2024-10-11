using System;

namespace ImmersiveGalleryAI.Common.AudioSystem
{
    public interface IAudioSystem
    {
        event Action PlayAudioClick;
        event Action PlayAudioMusic;

        void PlayMusic();
        void PlayClickSfx();
    }
}
