using UnityEngine;

namespace ImmersiveGalleryAI.Common.PlayerLocation
{
    public class PlayerLocationManager : IPlayerLocation 
    {
        public Transform CameraRigTransform { get; private set; }

        public void Init(PlayerLocationData playerLocationData)
        {
            CameraRigTransform = playerLocationData.CameraRigTransform;
        }
    }
}