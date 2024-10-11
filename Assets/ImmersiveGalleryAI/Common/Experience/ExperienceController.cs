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

        [Space] [SerializeField] private GameObject _webData;
        [SerializeField] private GameObject _applicationSettings;
        [SerializeField] private GameObject _teleportationArea;
        
        [Inject] private IExperienceManager _experienceManager;

        private void Awake()
        {
            _lobbyPanel.SetActive(true);
            _webData.SetActive(false);
            _applicationSettings.SetActive(false);
            _experiencePanel.SetActive(false);
            _teleportationArea.SetActive(false);
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
            
            _webData.SetActive(true);
            _applicationSettings.SetActive(true);
            _lobbyPanel.SetActive(false);
            _experiencePanel.SetActive(true);
            _teleportationArea.SetActive(true);
        }
    }
}
