using System;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.PlayerLocation
{
    public class PlayerLocationManager : IPlayerLocation 
    {
        public event Action PlayerTeleportedEvent;
        public Transform CameraRigTransform { get; private set; }

        public void Init(PlayerLocationData playerLocationData)
        {
            CameraRigTransform = playerLocationData.CameraRigTransform;
        }

        public void PlayerTeleported()
        {
            PlayerTeleportedEvent?.Invoke();
        }
    }
}