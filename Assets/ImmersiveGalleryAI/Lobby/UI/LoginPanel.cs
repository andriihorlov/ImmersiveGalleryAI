using System;
using ImmersiveGalleryAI.Main.Credits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class LoginPanel : UiPanel
    {
        private const int DefaultGuestCreditBalance = 5;
        
        public event Action RegistrationClickedEvent;
        public event Action ForgetPasswordClickedEvent;
        public event Action GuestClickedEvent;
        public event Action<string, string> LoginClickedEvent;

        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _registrationButton;
        [SerializeField] private Button _forgetPasswordButton;
        [SerializeField] private Button _guestButton;

        [Inject] private ICredits _credits;

        private void OnEnable()
        {
            _loginButton.onClick.AddListener(LoginButtonPressed);
            _registrationButton.onClick.AddListener(RegistrationButtonPressed);
            _forgetPasswordButton.onClick.AddListener(ForgetPasswordButtonPressed);
            _guestButton.onClick.AddListener(GuestButtonPressed);
        }

        private void OnDisable()
        {
            _loginButton.onClick.RemoveListener(LoginButtonPressed);
            _registrationButton.onClick.RemoveListener(RegistrationButtonPressed);
            _forgetPasswordButton.onClick.RemoveListener(ForgetPasswordButtonPressed);
            _guestButton.onClick.RemoveListener(GuestButtonPressed);
        }

        private void LoginButtonPressed()
        {
            _credits.SetCreditsBalance(DefaultGuestCreditBalance); // todo: change to the actual user balance
            LoginClickedEvent?.Invoke(_loginInputField.text, _passwordInputField.text);
        }

        private void RegistrationButtonPressed()
        {
            RegistrationClickedEvent?.Invoke();
        }

        private void GuestButtonPressed()
        {
            _credits.SetCreditsBalance(DefaultGuestCreditBalance);
            GuestClickedEvent?.Invoke();
        }

        private void ForgetPasswordButtonPressed()
        {
            ForgetPasswordClickedEvent?.Invoke();
        }

#if UNITY_EDITOR
        public void LoginEditor(string login, string password)
        {
            _loginInputField.text = login;
            _passwordInputField.text = password;
            LoginButtonPressed();
        }

        public void RegistrationEditor()
        {
            RegistrationButtonPressed();
        }

        public void ForgetPasswordEditor()
        {
            ForgetPasswordButtonPressed();
        }
        
        public void GuestModeEditor()
        {
            GuestButtonPressed();
        }
#endif
    }
}