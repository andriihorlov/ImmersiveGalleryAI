using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.SceneManagement;
using ImmersiveGalleryAI.Common.Settings;
using ImmersiveGalleryAI.Common.User;
using ImmersiveGalleryAI.Main.Credits;
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
        //[Inject] private ISettings _settings;
        [Inject] private ICredits _credits;
        [Inject] private IUser _user;

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
            _registrationPanel.GuestButtonEvent += GuestLoginEventHandler;
            _recoveryPanel.BackToLoginEvent += ForgetPanelBackEventHandler;
        }

        private void OnDisable()
        {
            _loginPanel.RegistrationClickedEvent -= InvokeRegistration;
            _loginPanel.ForgetPasswordClickedEvent -= InvokeRecoveryPanel;
            _loginPanel.LoginClickedEvent -= LoginEventHandler;
            _loginPanel.GuestClickedEvent -= GuestLoginEventHandler;
            _registrationPanel.BackToLoginEvent -= BackToLoginPanel;
            _registrationPanel.GuestButtonEvent -= GuestLoginEventHandler;
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
            ContinueAsGuest();
        }

        private async void ContinueAsGuest()
        {
            await _backend.GuestEnter();
            UserModel userModel = await _backend.GetUserModel(SystemInfo.deviceUniqueIdentifier);
            Debug.Log($"User: {userModel}; Login: {userModel?.login}");
            _user.SetUserData(userModel);
            
            Debug.Log($"Login: {_user.GetCurrentUserLogin()}");
            LoadMainScene();
        }

        private async void TryLoggedIn(string login, string password)
        {
            bool isLoggedSucceed = await _backend.Login(login, password);
            if (isLoggedSucceed)
            {
                UserModel userModel = await _backend.GetUserModel(login);
                _user.SetUserData(userModel);
                LoadMainScene();
            }
        }

        private void LoadMainScene()
        {
            _sceneManager.LoadScene(SceneType.Main);
        }

        // private void SetCredits()
        // {
        //     int creditBalance = _settings.GetDefaultImageCount();
        //     _credits.SetCreditsBalance(creditBalance);
        // }

        [ContextMenu("Check database")]
        private void CheckDatabase()
        {
            _backend.GetApplicationSettings();
        }
    }
}