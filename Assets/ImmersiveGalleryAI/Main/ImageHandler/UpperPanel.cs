using ImmersiveGalleryAI.Main.Credits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ImmersiveGalleryAI.Main.ImageHandler
{
    public class UpperPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _creditBalanceText;
        [SerializeField] private Button _upgradeButton;

        [Inject] private ICredits _credits;

        private void Start()
        {
            UpdateBalance(_credits.GetCreditsBalance());
        }

        private void OnEnable()
        {
            _upgradeButton.onClick.AddListener(UpgradeButtonClicked);
            _credits.UpdateBalanceEvent += UpdateBalance;
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(UpgradeButtonClicked);
            _credits.UpdateBalanceEvent -= UpdateBalance;
        }

        private void UpdateBalance(int credit)
        {
            _creditBalanceText.text = credit.ToString();
        }

        private void UpgradeButtonClicked()
        {
            _credits.UpgradeBalance();
        }
    }
}
