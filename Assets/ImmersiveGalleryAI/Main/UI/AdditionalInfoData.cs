using System;
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

    [Serializable]
    public class AdditionalInfoData
    {
        public AdditionalInfoType InfoType;
        public Sprite InfoSprite;
        public string InfoDescription;
    }
}