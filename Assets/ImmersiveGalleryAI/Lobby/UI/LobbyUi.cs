using ImmersiveGalleryAI.Common.Backend;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Lobby.UI
{
    public class LobbyUi : MonoBehaviour
    {
        [SerializeField] private LoginPanel _loginPanel;
        [SerializeField] private RegistrationPanel _registrationPanel;
        [SerializeField] private ForgetPanel _recoveryPanel;

        [Inject] private IBackend _backend;

        private void Awake()
        {
            _loginPanel.SetActive(true);
            _registrationPanel.SetActive(false);
            _recoveryPanel.SetActive(false);
        }

        private void OnEnable()
        {
            _loginPanel.RegistrationClickedEvent += InvokeRegistration;
            _loginPanel.ForgetPasswordClickedEvent += InvokeRecoveryPanel;
            _loginPanel.LoginClickedEvent += LoginEventHandler;
            _registrationPanel.BackToLoginEvent += BackToLoginPanel;
        }

        private void OnDisable()
        {
            _loginPanel.RegistrationClickedEvent -= InvokeRegistration;
            _loginPanel.ForgetPasswordClickedEvent -= InvokeRecoveryPanel;
            _loginPanel.LoginClickedEvent -= LoginEventHandler;
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
        
        private void InvokeRecoveryPanel()
        {
            _recoveryPanel.SetActive(true);
            _loginPanel.SetCanvasInteractables(false);
        }
        
        private void LoginEventHandler(string login, string password)
        {
            _backend.Login(login, password);
        }
    }
}