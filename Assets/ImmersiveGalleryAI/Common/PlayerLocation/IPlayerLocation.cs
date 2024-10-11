using System;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.PlayerLocation
{
    public interface IPlayerLocation
    {
        event Action PlayerTeleportedEvent;
        Transform CameraRigTransform { get; }
        
        void Init(PlayerLocationData playerLocationData);
        void PlayerTeleported();
    }
}
