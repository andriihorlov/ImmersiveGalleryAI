using ImmersiveGalleryAI.Common.Backend;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class ForgetPanel : UiPanel
    {
        [SerializeField] private Button _forgetButton;
        [SerializeField] private TMP_InputField _recoverEmail;

        [Inject] private IBackend _backend;

        private void OnEnable()
        {
            _forgetButton.onClick.AddListener(RecoverButtonClicked);
        }

        private void OnDisable()
        {
            _forgetButton.onClick.RemoveListener(RecoverButtonClicked);
        }

        private void RecoverButtonClicked()
        {
            _backend.RecoverPassword(_recoverEmail.text);
        }

#if UNITY_EDITOR
        public void RecoveryPasswordEditor(string email)
        {
            _recoverEmail.text = email;
            RecoverButtonClicked();
        }
#endif
    }
}