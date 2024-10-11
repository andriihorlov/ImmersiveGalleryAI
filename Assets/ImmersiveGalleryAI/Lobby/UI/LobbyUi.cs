using System.Text;
using ImmersiveGalleryAI.Common.AudioSystem;
using ImmersiveGalleryAI.Common.Backend;
using ImmersiveGalleryAI.Common.Experience;
using ImmersiveGalleryAI.Common.Keyboard;
using ImmersiveGalleryAI.Common.PlayerLocation;
using ImmersiveGalleryAI.Common.Settings;
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
        [SerializeField] private GameObject _lobbyWalls;

        [Inject] private IBackend _backend;
        [Inject] private IExperienceManager _experienceManager;
        [Inject] private IKeyboard _keyboard;
        [Inject] private IPlayerLocation _playerLocation;

        [Inject] private ISettings _settings;
        [Inject] private ICredits _credits;
        [Inject] private IUser _user;
        [Inject] private IAudioSystem _audioSystem;

        private void Awake()
        {
            _loginPanel.SetActiveImmediate(false);
            _registrationPanel.SetActiveImmediate(false);
            _recoveryPanel.SetActiveImmediate(false);

            InitUserName();
        }

        private void Start()
        {
            _loginPanel.SetActive(true);
            _lobbyWalls.SetActive(true);
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
            _loginPanel.RegistrationClickedEvent += RegistrationLoginClickEventHandler;
            _loginPanel.ForgetPasswordClickedEvent += RecoveryLoginClickPanel;
            _loginPanel.LoginClickedEvent += LoginClickEventHandler;
            _loginPanel.GuestClickedEvent += GuestLoginClickEventHandler;
            _registrationPanel.BackToLoginEvent += BackRegistrationClickPanel;
            _registrationPanel.GuestButtonEvent += GuestRegistrationClickEventHandler;
            _recoveryPanel.BackToLoginEvent += BackRecoveryClickEventHandler;

            _registrationPanel.RegistratedEvent += RegistratedEventHandler;

            _loginPanel.InputFieldSelectedEvent += LoginPanelInputFieldEventHandler;
            _registrationPanel.InputFieldSelectedEvent += RegistrationPanelInputFieldEventHandler;
            _recoveryPanel.InputFieldSelectedEvent += RecoveryPanelInputFieldEventHandler;
        }

        private void OnDisable()
        {
            _loginPanel.RegistrationClickedEvent -= RegistrationLoginClickEventHandler;
            _loginPanel.ForgetPasswordClickedEvent -= RecoveryLoginClickPanel;
            _loginPanel.LoginClickedEvent -= LoginClickEventHandler;
            _loginPanel.GuestClickedEvent -= GuestLoginClickEventHandler;
            _registrationPanel.BackToLoginEvent -= BackRegistrationClickPanel;
            _registrationPanel.GuestButtonEvent -= GuestRegistrationClickEventHandler;
            _recoveryPanel.BackToLoginEvent -= BackRecoveryClickEventHandler;

            _registrationPanel.RegistratedEvent -= RegistratedEventHandler;

            _loginPanel.InputFieldSelectedEvent -= LoginPanelInputFieldEventHandler;
            _registrationPanel.InputFieldSelectedEvent -= RegistrationPanelInputFieldEventHandler;
            _recoveryPanel.InputFieldSelectedEvent -= RecoveryPanelInputFieldEventHandler;
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void RegistrationLoginClickEventHandler()
        {
            PlayClickSfx();
            _loginPanel.SetActive(false);
            _registrationPanel.SetActive(true);
        }

        private void BackRegistrationClickPanel()
        {
            PlayClickSfx();
            _registrationPanel.SetActive(false);
            _loginPanel.SetActive(true);
        }

        private void BackRecoveryClickEventHandler()
        {
            PlayClickSfx();
            _recoveryPanel.SetActive(false);
            _loginPanel.SetCanvasInteractables(true);
        }

        private void RegistratedEventHandler(string login, string password)
        {
            PlayClickSfx();
            _loginPanel.InitPlayerName(login, password);
            _registrationPanel.SetActive(false);
            _loginPanel.SetActive(true);
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

        private void RecoveryLoginClickPanel()
        {
            PlayClickSfx();
            _recoveryPanel.SetActive(true);
            _loginPanel.SetCanvasInteractables(false);
        }

        private void LoginClickEventHandler(string login, string password)
        {
            PlayClickSfx();
            TryLoggedIn(login, password);
            _keyboard.SetActive(false);
        }

        private void GuestLoginClickEventHandler()
        {
            PlayClickSfx();
            ContinueAsGuest();
            _keyboard.SetActive(false);
        }

        private void GuestRegistrationClickEventHandler()
        {
            PlayClickSfx();
            ContinueAsGuest();
            _keyboard.SetActive(false);
        }

        private async void ContinueAsGuest()
        {
            await _backend.GuestEnter();
            UserModel userModel = await _backend.GetUserModel(SystemInfo.deviceUniqueIdentifier);
            Debug.Log($"User: {userModel}; Login: {userModel?.login}");
            userModel ??= new UserModel()
            {
                email = null,
                imageSettings = null,
                imagesLeft = _settings.GetDefaultImageCount(),
                login = "Guest" + SystemInfo.deviceUniqueIdentifier
            };
            
            _user.SetUserData(userModel);
            _user.IsGuest = true;

            Debug.Log($"Login: {_user.GetCurrentUserLogin()}");
            LoadNextPhase();
        }

        private async void TryLoggedIn(string login, string password)
        {
            login = login.ToLower();
            
            bool isLoggedSucceed = await _backend.Login(login, password);
            if (!isLoggedSucceed)
            {
                return;
            }

            UserModel userModel = await _backend.GetUserModel(login);
            _user.SetUserData(userModel);
            _user.IsGuest = false;

            LoadNextPhase();
            PlayerPrefs.SetString(PlayerNamePrefs, login);
            PlayerPrefs.SetString(PlayerPasswordPrefs, password);
        }

        private void LoadNextPhase()
        {
            _lobbyWalls.SetActive(false);
            _experienceManager.LoginSuccess(ExperiencePhase.Main);
        }

        private void PlayClickSfx() => _audioSystem.PlayClickSfx();
        
        [ContextMenu("Check database")]
        private void CheckDatabase()
        {
            _backend.GetApplicationSettings();
        }
    }
}