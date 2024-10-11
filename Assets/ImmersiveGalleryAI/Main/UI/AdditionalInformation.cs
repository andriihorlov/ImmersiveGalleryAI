using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.Main.UI
{
    public class AdditionalInformation : MonoBehaviour
    {
        private const string DefaultAiErrorCredits = "Billing hard limit has been reached";
        private const string DefaultAiErrorUnSafetyPrompt = "Your request was rejected as a result of our safety system. Your prompt may contain text that is not allowed by our safety system.";
        
        [SerializeField] private Image _additionalDataIcon;
        [SerializeField] private TextMeshProUGUI _additionalText;
        [SerializeField] private GameObject _additionalData;
        [SerializeField] private List<AdditionalInfoData> _additionalDataList;

        public void SetActive(bool isActive, AdditionalInfoType infoType = AdditionalInfoType.None)
        {
            if (isActive && infoType == AdditionalInfoType.None)
            {
                Debug.LogWarning($"Please, provide the info type!");
                return;
            }

            if (isActive)
            {
                SetInformation(infoType);
            }
            
            _additionalData.SetActive(isActive);
        }
        
        public void SetActive(bool isActive, string message)
        {
            if (isActive)
            {
                AdditionalInfoType additionalMessage = GetAdditionalInfoType(message);
                if (additionalMessage == AdditionalInfoType.None)
                {
                    _additionalText.text = message;
                    _additionalDataIcon.enabled = false;
                }
                else
                {
                    SetInformation(additionalMessage);
                }
            }
            
            _additionalData.SetActive(isActive);
        }

        private void SetInformation(AdditionalInfoType infoType)
        {
            AdditionalInfoData infoData = GetInfoData(infoType);
            if (infoData == null)
            {
                Debug.LogError($"Impossible to find needed information! {infoType.ToString()}");
                return;
            }

            _additionalDataIcon.sprite = infoData.InfoSprite;
            _additionalText.text = infoData.InfoDescription;
            _additionalDataIcon.enabled = true;
        }

        private AdditionalInfoType GetAdditionalInfoType(string message)
        {
            if (message == DefaultAiErrorCredits)
            {
                return AdditionalInfoType.NoCredits;
            }

            if (message == DefaultAiErrorUnSafetyPrompt)
            {
                return AdditionalInfoType.InappropriateText;
            }

            Debug.Log($"The system doesn't know what is: <b>{message}</b>");
            return AdditionalInfoType.None;
        }
        
        private AdditionalInfoData GetInfoData(AdditionalInfoType infoData) => _additionalDataList.Find(t => t.InfoType == infoData);
    }
}
