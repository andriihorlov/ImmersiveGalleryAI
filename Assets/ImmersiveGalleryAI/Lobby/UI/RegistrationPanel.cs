using System;
using ImmersiveGalleryAI.Common.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class RegistrationPanel : UiPanel
    {
        public event Action BackToLoginEvent;
        public event Action GuestButtonEvent;
        public event Action<TMP_InputField> InputFieldSelectedEvent;

        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_InputField _repeatPasswordInputField;

        [SerializeField] private Button _registrationButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _guestButton;

        private bool _isPasswordOk;

        [Inject] private IBackend _backend;

        private void OnEnable()
        {
            _registrationButton.onClick.AddListener(RegistrationButtonClicked);
            _backButton.onClick.AddListener(BackButtonClicked);
            _guestButton.onClick.AddListener(GuestButtonClicked);
            _repeatPasswordInputField.onDeselect.AddListener(RepeatPasswordDeselect);
            
            _emailInputField.onSelect.AddListener(EmailSelected);
            _loginInputField.onSelect.AddListener(LoginSelected);
            _passwordInputField.onSelect.AddListener(PasswordSelected);
            _repeatPasswordInputField.onSelect.AddListener(PasswordRepeatSelected);
        }

        private void OnDisable()
        {
            _registrationButton.onClick.RemoveListener(RegistrationButtonClicked);
            _backButton.onClick.RemoveListener(BackButtonClicked);
            _guestButton.onClick.RemoveListener(GuestButtonClicked);
            _repeatPasswordInputField.onDeselect.RemoveListener(RepeatPasswordDeselect);
            
            
            _emailInputField.onSelect.RemoveListener(EmailSelected);
            _loginInputField.onSelect.RemoveListener(LoginSelected);
            _passwordInputField.onSelect.RemoveListener(PasswordSelected);
            _repeatPasswordInputField.onSelect.RemoveListener(PasswordRepeatSelected);
        }

        private void RepeatPasswordDeselect(string repeatedPassword)
        {
            _isPasswordOk = repeatedPassword == _passwordInputField.text;
        }

        private void RegistrationButtonClicked()
        {
            if (!_isPasswordOk)
            {
                _isPasswordOk = _repeatPasswordInputField.text == _passwordInputField.text;
                if (!_isPasswordOk)
                {
                    Debug.Log($"Passwords not the same");
                    return;
                }
            }

            if (_emailInputField.text == string.Empty)
            {
                Debug.Log($"Email shouldn't be empty");
                return;
            }

            if (_loginInputField.text == string.Empty)
            {
                Debug.Log($"Login shouldn't be empty");
                return;
            }

            RegistrationHandler();
        }

        private void GuestButtonClicked()
        {
            GuestButtonEvent?.Invoke();
        }

        private async void RegistrationHandler()
        {
            bool isSuccess = await _backend.Registration(_loginInputField.text, _emailInputField.text, _passwordInputField.text);
            if (isSuccess)
            {
                BackButtonClicked();
                return;
            }

            Debug.LogError($"Registration wasn't succeed.");
        }

        private void BackButtonClicked()
        {
            BackToLoginEvent?.Invoke();
        }

        private void EmailSelected(string arg0)
        {
            InputFieldSelectedEvent?.Invoke(_emailInputField);
        }
        
        private void LoginSelected(string arg0)
        {
            InputFieldSelectedEvent?.Invoke(_loginInputField);
        }

        private void PasswordSelected(string arg0)
        {
            InputFieldSelectedEvent?.Invoke(_passwordInputField);
        }

        private void PasswordRepeatSelected(string arg0)
        {
            InputFieldSelectedEvent?.Invoke(_repeatPasswordInputField);
        }

#if UNITY_EDITOR

        public void UpdateValuesEditor(string email, string login, string password)
        {
            _loginInputField.text = login;
            _emailInputField.text = email;
            _passwordInputField.text = password;
            _repeatPasswordInputField.text = password;
        }

        public void RegistrationEditor()
        {
            RegistrationButtonClicked();
        }

        public void BackEditor()
        {
            BackButtonClicked();
        }

        public void GuestEditor()
        {
            GuestButtonClicked();
        }

        [ContextMenu("Fill random values")]
        public void FillRandom()
        {
            byte maxCharacters = 8;
            string login = "User_" + Random.Range(0, byte.MaxValue);
            _loginInputField.text = login;
            if (login.Length > maxCharacters)
            {
                login = login.Substring(maxCharacters);
            }

            _emailInputField.text = login + "@fake.com";
            _passwordInputField.text = login + "!!!";
            _repeatPasswordInputField.text = _passwordInputField.text;
        }

        public RegistrationData GetValues()
        {
            return new RegistrationData {Email = _emailInputField.text, Login = _loginInputField.text, Password = _passwordInputField.text};
        }

        [Serializable]
        public class RegistrationData
        {
            public string Email;
            public string Login;
            public string Password;
        }
#endif
    }
}