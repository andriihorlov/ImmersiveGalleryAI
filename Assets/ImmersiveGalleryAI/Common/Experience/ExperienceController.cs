using System;
using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.Settings;
using ImmersiveGalleryAI.Lobby.UI;
using ImmersiveGalleryAI.Main;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.Experience
{
    public class ExperienceController : MonoBehaviour
    {
        [Tooltip("Count of image places")]
        [SerializeField] public int _wallImages = 11;
        
        [Space]
        [SerializeField] private LobbyUi _lobbyPanel;
        [SerializeField] private ImagesController _experiencePanel;

        [Space] [SerializeField] private GameObject _webData;
        [SerializeField] private GameObject _applicationSettings;
        [SerializeField] private GameObject _teleportationArea;
        
        [Inject] private IExperienceManager _experienceManager;
        [Inject] private IBackend _backend;
        [Inject] private ISettings _settings;

        private void Awake()
        {
            _lobbyPanel.SetActive(true);
            _webData.SetActive(false);
            _applicationSettings.SetActive(false);
            _experiencePanel.SetActive(false);
            _teleportationArea.SetActive(false);
        }

        private void Start()
        {
            InitSettings();
        }

        private async void InitSettings()
        {
            SettingsData settingsData = await _backend.GetApplicationSettings();
            _settings.SetSettings(settingsData);
            _backend.SetWallImagesCount(_wallImages);
        }

        private void OnEnable()
        {
            _experienceManager.LoginSuccessEvent += LoginSuccessEventHandler;
        }

        private void OnDisable()
        {
            _experienceManager.LoginSuccessEvent -= LoginSuccessEventHandler;
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
