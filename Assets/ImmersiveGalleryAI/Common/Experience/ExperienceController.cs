using ImmersiveGalleryAI.Lobby.UI;
using ImmersiveGalleryAI.Main;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.Experience
{
    public class ExperienceController : MonoBehaviour
    {
        [SerializeField] private LobbyUi _lobbyPanel;
        [SerializeField] private ImagesController _experiencePanel;
        
        [Inject] private IExperienceManager _experienceManager;

        private void Awake()
        {
            _lobbyPanel.SetActive(true);
            _experiencePanel.SetActive(false);
        }

        private void OnEnable()
        {
            _experienceManager.LoginSuccessEvent += LoginSuccessEventHandler;
        }

        private void LoginSuccessEventHandler(ExperiencePhase experiencePhase)
        {
            if (experiencePhase != ExperiencePhase.Main)
            {
                return;
            }
            
            _lobbyPanel.SetActive(false);
            _experiencePanel.SetActive(true);
        }
    }
}
