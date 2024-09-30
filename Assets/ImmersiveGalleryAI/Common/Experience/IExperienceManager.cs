using System;

namespace ImmersiveGalleryAI.Common.Experience
{
    public interface IExperienceManager
    {
        event Action<ExperiencePhase> LoginSuccessEvent;
        void LoginSuccess(ExperiencePhase experiencePhase);
    }
}
