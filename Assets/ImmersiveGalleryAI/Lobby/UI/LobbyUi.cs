using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.SceneManagement;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class LobbyUi : MonoBehaviour
    {
        [SerializeField] private LoginPanel _loginPanel;
        [SerializeField] private RegistrationPanel _registrationPanel;
        [SerializeField] private ForgetPanel _recoveryPanel;

        [Inject] private IBackend _backend;
        [Inject] private ISceneManager _sceneManager;

        private void Awake()
        {
            _loginPanel.SetActive(true);
            _registrationPanel.SetActive(false);
            _recoveryPanel.SetActive(false);
        }

        private void OnEnable()
        {
            _loginPanel.RegistrationClickedEvent += InvokeRegistration;
            _loginPanel.ForgetPasswordClickedEvent += InvokeRecoveryPanel;
            _loginPanel.LoginClickedEvent += LoginEventHandler;
            _loginPanel.GuestClickedEvent += GuestLoginEventHandler;
            _registrationPanel.BackToLoginEvent += BackToLoginPanel;
            _recoveryPanel.BackToLoginEvent += ForgetPanelBackEventHandler;
        }

        private void OnDisable()
        {
            _loginPanel.RegistrationClickedEvent -= InvokeRegistration;
            _loginPanel.ForgetPasswordClickedEvent -= InvokeRecoveryPanel;
            _loginPanel.LoginClickedEvent -= LoginEventHandler;
            _loginPanel.GuestClickedEvent -= GuestLoginEventHandler;
            _registrationPanel.BackToLoginEvent -= BackToLoginPanel;
            _recoveryPanel.BackToLoginEvent -= ForgetPanelBackEventHandler;
        }

        private void InvokeRegistration()
        {
            _loginPanel.SetActive(false);
            _registrationPanel.SetActive(true);
        }

        private void BackToLoginPanel()
        {
            _registrationPanel.SetActive(false);
            _loginPanel.SetActive(true);
        }

        private void ForgetPanelBackEventHandler()
        {
            _recoveryPanel.SetActive(false);
            _loginPanel.SetCanvasInteractables(true);
        }

        private void InvokeRecoveryPanel()
        {
            _recoveryPanel.SetActive(true);
            _loginPanel.SetCanvasInteractables(false);
        }

        private void LoginEventHandler(string login, string password)
        {
            TryLoggedIn(login, password);
        }

        private void GuestLoginEventHandler()
        {
            LoadMainScene();
        }

        private async void TryLoggedIn(string login, string password)
        {
            bool isLoggedSucceed = await _backend.Login(login, password);
            if (isLoggedSucceed)
            {
                LoadMainScene();
            }
        }

        private void LoadMainScene()
        {
            _sceneManager.LoadScene(SceneType.Main);
        }
    }
}