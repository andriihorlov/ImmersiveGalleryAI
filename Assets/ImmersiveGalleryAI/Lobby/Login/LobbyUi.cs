using UnityEngine;

namespace ImmersiveGalleryAI.Lobby.Login
{
    public class LobbyUi : MonoBehaviour
    {
        [SerializeField] private LoginPanel _loginPanel;
        [SerializeField] private RegistrationPanel _registrationPanel;

        private void Awake()
        {
            _loginPanel.SetActive(true);
            _registrationPanel.SetActive(false);
        }

        private void OnEnable()
        {
            _loginPanel.RegistrationClickedEvent += InvokeRegistration;
            _registrationPanel.BackToLoginEvent += BackToLoginPanel;
        }

        private void OnDisable()
        {
            _loginPanel.RegistrationClickedEvent -= InvokeRegistration;
            _registrationPanel.BackToLoginEvent -= BackToLoginPanel;
        }

        private void InvokeRegistration()
        {
            _loginPanel.SetActive(false);
            _registrationPanel.SetActive(true);
        }

        private void BackToLoginPanel()
        {
            _registrationPanel.SetActive(false);
            _loginPanel.SetActive(true);
        }
    }
}