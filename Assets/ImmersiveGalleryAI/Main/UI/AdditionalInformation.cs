using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.Main.UI
{
    public class AdditionalInformation : MonoBehaviour
    {
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
        }

        private AdditionalInfoData GetInfoData(AdditionalInfoType infoData) => _additionalDataList.Find(t => t.InfoType == infoData);
    }
}
