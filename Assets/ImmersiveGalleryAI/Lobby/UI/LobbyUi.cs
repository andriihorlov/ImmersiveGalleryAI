using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.Experience;
using ImmersiveGalleryAI.Common.Keyboard;
using ImmersiveGalleryAI.Common.PlayerLocation;
using ImmersiveGalleryAI.Common.User;
using ImmersiveGalleryAI.Main.Credits;
using TMPro;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class LobbyUi : MonoBehaviour
    {
        private const string PlayerNamePrefs = "PlayerName";
        private const string PlayerPasswordPrefs = "Password";

        [SerializeField] private LoginPanel _loginPanel;
        [SerializeField] private RegistrationPanel _registrationPanel;
        [SerializeField] private ForgetPanel _recoveryPanel;

        [Inject] private IBackend _backend;
        [Inject] private IExperienceManager _experienceManager;
        [Inject] private IKeyboard _keyboard;
        [Inject] private IPlayerLocation _playerLocation;

        //[Inject] private ISettings _settings;
        [Inject] private ICredits _credits;
        [Inject] private IUser _user;

        private void Awake()
        {
            _loginPanel.SetActive(true);
            _registrationPanel.SetActive(false);
            _recoveryPanel.SetActive(false);

            InitUserName();
        }

        private void InitUserName()
        {
            string playerName = PlayerPrefs.GetString(PlayerNamePrefs);
            if (string.IsNullOrEmpty(playerName))
            {
                return;
            }

            string password = PlayerPrefs.GetString(PlayerPasswordPrefs);
            if (string.IsNullOrEmpty(password))
            {
                Debug.LogError($"Can't find saved password.");
                return;
            }

            _loginPanel.InitPlayerName(playerName, password);
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

            _loginPanel.InputFieldSelectedEvent += LoginPanelInputFieldEventHandler;
            _registrationPanel.InputFieldSelectedEvent += RegistrationPanelInputFieldEventHandler;
            _recoveryPanel.InputFieldSelectedEvent += RecoveryPanelInputFieldEventHandler;
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

            _loginPanel.InputFieldSelectedEvent -= LoginPanelInputFieldEventHandler;
            _registrationPanel.InputFieldSelectedEvent -= RegistrationPanelInputFieldEventHandler;
            _recoveryPanel.InputFieldSelectedEvent -= RecoveryPanelInputFieldEventHandler;
        }

        public void SetActive(bool isActive)
        {
            _loginPanel.SetActive(isActive);
            _registrationPanel.SetActive(isActive);
            _recoveryPanel.SetActive(isActive);
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

        private void LoginPanelInputFieldEventHandler(TMP_InputField tmpInputField)
        {
            SetActiveKeyboard(true);
            _keyboard.Target = tmpInputField;
        }

        private void RecoveryPanelInputFieldEventHandler(TMP_InputField tmpInputField)
        {
            SetActiveKeyboard(true);
            _keyboard.Target = tmpInputField;
        }

        private void RegistrationPanelInputFieldEventHandler(TMP_InputField tmpInputField)
        {
            SetActiveKeyboard(true);
            _keyboard.Target = tmpInputField;
        }

        private void SetActiveKeyboard(bool isActive)
        {
            if (_keyboard.IsActive == isActive)
            {
                return;
            }

            _keyboard.ChangePosition(_playerLocation.CameraRigTransform);
            _keyboard.SetActive(true);
        }

        private void InvokeRecoveryPanel()
        {
            _recoveryPanel.SetActive(true);
            _loginPanel.SetCanvasInteractables(false);
        }

        private void LoginEventHandler(string login, string password)
        {
            TryLoggedIn(login, password);
            _keyboard.SetActive(false);
        }

        private void GuestLoginEventHandler()
        {
            ContinueAsGuest();
            _keyboard.SetActive(false);
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
                PlayerPrefs.SetString(PlayerNamePrefs, login);
            }
        }

        private void LoadMainScene()
        {
            _experienceManager.LoginSuccess(ExperiencePhase.Main);
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