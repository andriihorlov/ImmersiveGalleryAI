﻿using System;
using ImmersiveGalleryAI.Common.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.Lobby.Login
{
    public class RegistrationPanel : UiPanel
    {
        public event Action BackToLoginEvent;

        [SerializeField] private TMP_InputField _emailInputField;
        [SerializeField] private TMP_InputField _loginInputField;
        [SerializeField] private TMP_InputField _passwordInputField;
        [SerializeField] private TMP_InputField _repeatPasswordInputField;

        [SerializeField] private Button _registrationButton;
        [SerializeField] private Button _backButton;

        private bool _isPasswordOk;

        [Inject] private IBackend _backend;

        private void OnEnable()
        {
            _registrationButton.onClick.AddListener(RegistrationButtonClicked);
            _backButton.onClick.AddListener(BackButtonClicked);

            _repeatPasswordInputField.onDeselect.AddListener(RepeatPasswordDeselect);
        }

        private void OnDisable()
        {
            _registrationButton.onClick.RemoveListener(RegistrationButtonClicked);
            _backButton.onClick.RemoveListener(BackButtonClicked);

            _repeatPasswordInputField.onDeselect.RemoveListener(RepeatPasswordDeselect);
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

            _backend.Registration(_emailInputField.text, _passwordInputField.text);

            BackButtonClicked();
        }

        private void BackButtonClicked()
        {
            BackToLoginEvent?.Invoke();
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

#endif
    }
}