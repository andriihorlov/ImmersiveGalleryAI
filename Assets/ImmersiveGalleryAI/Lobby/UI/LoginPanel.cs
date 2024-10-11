using System;
using ImmersiveGalleryAI.Common.Settings;
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
        public event Action<TMP_InputField> InputFieldSelectedEvent;

        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _registrationButton;
        [SerializeField] private Button _forgetPasswordButton;
        [SerializeField] private Button _guestButton;

        private void OnEnable()
        {
            _loginButton.onClick.AddListener(LoginButtonPressed);
            _registrationButton.onClick.AddListener(RegistrationButtonPressed);
            _forgetPasswordButton.onClick.AddListener(ForgetPasswordButtonPressed);
            _guestButton.onClick.AddListener(GuestButtonPressed);
            
            _loginInputField.onSelect.AddListener(LoginInputFieldSelected);
            _passwordInputField.onSelect.AddListener(PasswordInputFieldSelected);
        }

        private void OnDisable()
        {
            _loginButton.onClick.RemoveListener(LoginButtonPressed);
            _registrationButton.onClick.RemoveListener(RegistrationButtonPressed);
            _forgetPasswordButton.onClick.RemoveListener(ForgetPasswordButtonPressed);
            _guestButton.onClick.RemoveListener(GuestButtonPressed);
            
            _loginInputField.onSelect.RemoveListener(LoginInputFieldSelected);
            _passwordInputField.onSelect.RemoveListener(PasswordInputFieldSelected);
        }

        public void InitPlayerName(string playerName, string password)
        {
            _loginInputField.text = playerName;
            _passwordInputField.text = password;
        }
        
        private void LoginButtonPressed()
        {
            LoginClickedEvent?.Invoke(_loginInputField.text, _passwordInputField.text);
        }

        private void RegistrationButtonPressed()
        {
            RegistrationClickedEvent?.Invoke();
        }

        private void GuestButtonPressed()
        {
            GuestClickedEvent?.Invoke();
        }

        private void ForgetPasswordButtonPressed()
        {
            ForgetPasswordClickedEvent?.Invoke();
        }

        private void LoginInputFieldSelected(string arg0)
        {
            InputFieldSelectedEvent?.Invoke(_loginInputField);
        }

        private void PasswordInputFieldSelected(string arg0)
        {
            InputFieldSelectedEvent?.Invoke(_passwordInputField);
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