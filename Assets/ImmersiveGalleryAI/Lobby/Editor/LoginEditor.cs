using ImmersiveGalleryAI.Lobby.Login;
using UnityEditor;
using UnityEngine;

namespace ImmersiveGalleryAI.Lobby.Editor
{
    [CustomEditor(typeof(LoginPanel))]
    public class LoginEditor : UnityEditor.Editor
    {
        private LoginPanel _loginPanel;
        private LoginPanel LoginPanel => _loginPanel ??= target as LoginPanel;
        
        private string _login;
        private string _password;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
            {
                return;
            }
            
            _login = EditorGUILayout.TextField("Login", _login);
            _password = EditorGUILayout.TextField("Password", _password);
            
            if (GUILayout.Button("Try Login"))
            {
                LoginPanel.LoginEditor(_login, _password);
            }
            
            if (GUILayout.Button("Registration"))
            {
                LoginPanel.RegistrationEditor();
            }
            
            if (GUILayout.Button("Forget password"))
            {
                LoginPanel.RegistrationEditor();
            }
        }
    }
}