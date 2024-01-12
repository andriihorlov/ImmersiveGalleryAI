using UnityEngine;

namespace ImmersiveGalleryAI.Common.PlayerLocation
{
    public interface IPlayerLocation 
    {
        Transform CameraRigTransform { get; }
        void Init(PlayerLocationData playerLocationData);
    }
}
