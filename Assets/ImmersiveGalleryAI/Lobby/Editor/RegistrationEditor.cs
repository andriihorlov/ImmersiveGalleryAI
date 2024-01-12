using ImmersiveGalleryAI.Lobby.UI;
using UnityEditor;
using UnityEngine;

namespace ImmersiveGalleryAI.Lobby.Editor
{
    [CustomEditor(typeof(RegistrationPanel))]
    public class RegistrationEditor : UnityEditor.Editor
    {
        private RegistrationPanel _registrationPanel;
        private RegistrationPanel RegistrationPanel => _registrationPanel ??= target as RegistrationPanel;

        private bool _isDefaultInspector;

        private string _email;
        private string _login;
        private string _password;
        private string _repeatPassword;

        public override void OnInspectorGUI()
        {
            if (!Application.isPlaying)
            {
                base.OnInspectorGUI();
                return;
            }

            _isDefaultInspector = EditorGUILayout.Toggle("Is default inspector?", _isDefaultInspector);
            
            if (_isDefaultInspector)
            {
                base.OnInspectorGUI();
            }

            DrawRegistrationPanel();
        }

        private void DrawRegistrationPanel()
        {
            _email = EditorGUILayout.TextField("Email", _email);
            _login = EditorGUILayout.TextField("Login", _login);
            _password = EditorGUILayout.TextField("Password", _password);

            if (GUILayout.Button("Update values"))
            {
                RegistrationPanel.UpdateValuesEditor(_email, _login, _password);
            }

            if (GUILayout.Button("Registration"))
            {
                RegistrationPanel.RegistrationEditor();
            }
            
            if (GUILayout.Button("Back"))
            {
                RegistrationPanel.BackEditor();
            }
            
        }
    }
}