using System;

namespace ImmersiveGalleryAI.Common.Experience
{
    public class ExperienceManager : IExperienceManager
    {
        public event Action<ExperiencePhase> LoginSuccessEvent;

        public void LoginSuccess(ExperiencePhase experiencePhase)
        {
            LoginSuccessEvent?.Invoke(experiencePhase);
        }
    }
}