using ImmersiveGalleryAI.Lobby.UI;
using UnityEditor;
using UnityEngine;

namespace ImmersiveGalleryAI.Lobby.Editor
{
    [CustomEditor(typeof(ForgetPanel))]
    public class RecoveryPasswordEditor : UnityEditor.Editor
    {
        private ForgetPanel _forgetPanel;
        private ForgetPanel ForgetPanel => _forgetPanel ??= target as ForgetPanel;
        
        private string _email;
        private string _password;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying)
            {
                return;
            }
            
            _email = EditorGUILayout.TextField("Email", _email);
            
            if (GUILayout.Button("Try recovery"))
            {
                ForgetPanel.RecoveryPasswordEditor(_email);
            }
        }
    }
}