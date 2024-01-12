using System;
using ImmersiveGalleryAI.Common.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class ForgetPanel : UiPanel
    {
        public event Action BackToLoginEvent;
        
        [SerializeField] private Button _forgetButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_InputField _recoverEmail;

        [Inject] private IBackend _backend;

        private void OnEnable()
        {
            _forgetButton.onClick.AddListener(RecoverButtonClicked);
            _closeButton.onClick.AddListener(CloseButtonClicked);
        }

        private void OnDisable()
        {
            _forgetButton.onClick.RemoveListener(RecoverButtonClicked);
            _closeButton.onClick.RemoveListener(CloseButtonClicked);
        }

        private void RecoverButtonClicked()
        {
            _backend.RecoverPassword(_recoverEmail.text);
        }

        private void CloseButtonClicked()
        {
            BackToLoginEvent?.Invoke();
        }

#if UNITY_EDITOR
        public void RecoveryPasswordEditor(string email)
        {
            _recoverEmail.text = email;
            RecoverButtonClicked();
        }
        
        public void ClosePanel()
        {
            CloseButtonClicked();
        }
#endif
    }
}