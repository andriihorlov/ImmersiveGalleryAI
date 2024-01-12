using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class LoginPanel : UiPanel
    {
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
#endif
    }
}