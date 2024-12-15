using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.Main.ImageHandler
{
    public class UpperPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _creditBalanceText;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _helpButton;

        public event Action RequestUpgradeBalance;
        
        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(UpgradeButtonClicked);
            _helpButton.onClick.AddListener(HelpButtonClicked);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(UpgradeButtonClicked);
            _helpButton.onClick.RemoveListener(HelpButtonClicked);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void UpdateBalance(int credit)
        {
            _creditBalanceText.text = credit.ToString();
        }

        private void UpgradeButtonClicked()
        {
            RequestUpgradeBalance?.Invoke();
        }
        
        private void HelpButtonClicked()
        {
            // todo: add functionality
        }
    }
}
