using System;
using UnityEditor;
using UnityEngine;

namespace ImmersiveGalleryAI.Main.UI
{
    public enum AdditionalInfoType
    {
        None = -1,
        Default,
        NoInternet,
        NoCredits,
        InappropriateText
    }

    [CreateAssetMenu(fileName = "Additional Data", menuName = "AdditionalInfo/Data")]
    [Serializable]
    public class AdditionalInfoData : ScriptableObject
    {
        public AdditionalInfoType InfoType;
        public Sprite InfoSprite;
        public string InfoDescription;
    }
}