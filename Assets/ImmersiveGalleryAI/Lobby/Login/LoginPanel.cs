using System;
using ImmersiveGalleryAI.Common.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.Lobby.Login
{
    public class LoginPanel : UiPanel
    {
        public event Action RegistrationClickedEvent;

        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private TMP_InputField _passwordInputField;

        [SerializeField] private Button _loginButton;
        [SerializeField] private Button _registrationButton;
        [SerializeField] private Button _forgetPasswordButton;

        [Inject] private IBackend _backend;

        private void OnEnable()
        {
            _loginButton.onClick.AddListener(LoginButtonPressed);
            _registrationButton.onClick.AddListener(RegistrationButtonPressed);
            _forgetPasswordButton.onClick.AddListener(ForgetPasswordButtonPressed);
        }

        private void OnDisable()
        {
            _loginButton.onClick.RemoveListener(LoginButtonPressed);
            _registrationButton.onClick.RemoveListener(RegistrationButtonPressed);
            _forgetPasswordButton.onClick.RemoveListener(ForgetPasswordButtonPressed);
        }

        private void LoginButtonPressed()
        {
            _backend.Login(_loginInputField.text, _passwordInputField.text);
        }

        private void RegistrationButtonPressed()
        {
            RegistrationClickedEvent?.Invoke();
        }

        private void ForgetPasswordButtonPressed()
        {
            Debug.Log($"Your password was sent to your email. (no)");
        }

#if UNITY_EDITOR
        public void LoginEditor(string login, string password)
        {
            _backend.Login(login, password);
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