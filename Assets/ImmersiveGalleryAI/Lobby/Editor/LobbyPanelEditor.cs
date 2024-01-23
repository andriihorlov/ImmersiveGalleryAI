using System;
using ImmersiveGalleryAI.Lobby.UI;
using UnityEditor;
using UnityEngine;

namespace ImmersiveGalleryAI.Lobby.Editor
{
    [CustomEditor(typeof(LobbyUi))]
    public class LobbyPanelEditor : UnityEditor.Editor
    {
        private enum PanelType
        {
            Login,
            Registration,
            PasswordRecovery
        }

        private const string DefaultUserName = "fidg";
        private const string DefaultUserPassword = "fidgfidg";

        private LoginPanel _loginPanel;
        private LoginPanel LoginPanel => _loginPanel ??= FindObjectOfType<LoginPanel>();

        private RegistrationPanel _registrationPanel;
        private RegistrationPanel RegistrationPanel => _registrationPanel ??= FindObjectOfType<RegistrationPanel>();

        private ForgetPanel _forgetPanel;
        private ForgetPanel ForgetPanel => _forgetPanel ??= FindObjectOfType<ForgetPanel>();

        private PanelType _currentPanelType = PanelType.Login;

        private string _startLogin = DefaultUserName;
        private string _startPassword = DefaultUserPassword;

        private string _registrationEmail;
        private string _registrationLogin;
        private string _registrationPassword;
        private string _registrationRepeatPassword;

        private string _recoveryEmail;
        private string _recoveryPassword;

        private bool _isCustomRegistration;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
            {
                return;
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            switch (_currentPanelType)
            {
                case PanelType.Login:
                    LoginPanelHandler();
                                  break;
                case PanelType.Registration:
                    RegistrationPanelHandler();
                    RegistrationPanel.BackToLoginEvent += RegistrationBackButtonHandler;
                    break;
                case PanelType.PasswordRecovery:
                    RecoveryPasswordPanelHandler();
                    ForgetPanel.BackToLoginEvent += ForgetPanelBackButtonHandler;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LoginPanelHandler()
        {
            _startLogin = EditorGUILayout.TextField("Login", _startLogin);
            _startPassword = EditorGUILayout.TextField("Password", _startPassword);

            if (GUILayout.Button("Try Login"))
            {
                LoginPanel.LoginEditor(_startLogin, _startPassword);
            }

            if (GUILayout.Button("Registration"))
            {
                LoginPanel.RegistrationEditor();
                _currentPanelType = PanelType.Registration;
            }

            if (GUILayout.Button("Forget password"))
            {
                LoginPanel.ForgetPasswordEditor();
                _currentPanelType = PanelType.PasswordRecovery;
            }

            if (GUILayout.Button("Guest mode"))
            {
                LoginPanel.GuestModeEditor();
            }
        }

        private void RegistrationPanelHandler()
        {
            _isCustomRegistration = EditorGUILayout.BeginFoldoutHeaderGroup(_isCustomRegistration, "Fill data settings");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField(_isCustomRegistration ? "Custom registration" : "Registration", EditorStyles.boldLabel);
            if (_isCustomRegistration)
            {
                _registrationEmail = EditorGUILayout.TextField("Email", _registrationEmail);
                _registrationLogin = EditorGUILayout.TextField("Login", _registrationLogin);
                _registrationPassword = EditorGUILayout.TextField("Password", _registrationPassword);

                if (GUILayout.Button("Update values"))
                {
                    RegistrationPanel.UpdateValuesEditor(_registrationEmail, _registrationLogin, _registrationPassword);
                }

                if (GUILayout.Button("Get values to editor"))
                {
                    FillCustomRegistrationFields();
                }
            }
            else
            {
                if (GUILayout.Button("Get random values"))
                {
                    RegistrationPanel.FillRandom();
                    FillCustomRegistrationFields();
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            if (GUILayout.Button("Registration"))
            {
                RegistrationPanel.RegistrationEditor();
            }

            if (GUILayout.Button("Back"))
            {
                RegistrationPanel.BackEditor();
                _currentPanelType = PanelType.Login;
            }
        }

        private void FillCustomRegistrationFields()
        {
            RegistrationPanel.RegistrationData values = RegistrationPanel.GetValues();
            _registrationEmail = values.Email;
            _registrationLogin = values.Login;
            _registrationPassword = values.Password;
        }

        private void RecoveryPasswordPanelHandler()
        {
            _recoveryEmail = EditorGUILayout.TextField("Email", _recoveryEmail);

            if (GUILayout.Button("Try recovery"))
            {
                ForgetPanel.RecoveryPasswordEditor(_recoveryEmail);
                _currentPanelType = PanelType.Login;
            }

            if (GUILayout.Button("Close"))
            {
                ForgetPanel.ClosePanel();
            }
        }

        private void RegistrationBackButtonHandler()
        {
            RegistrationPanel.BackToLoginEvent -= RegistrationBackButtonHandler;
            _currentPanelType = PanelType.Login;
        }

        private void ForgetPanelBackButtonHandler()
        {
            ForgetPanel.BackToLoginEvent -= ForgetPanelBackButtonHandler;
            _currentPanelType = PanelType.Login;
        }
    }
}