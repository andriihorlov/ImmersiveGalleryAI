using Unity.XR.CoreUtils;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.PlayerLocation
{
    public class PlayerLocationData : MonoBehaviour
    {
        [SerializeField] private XROrigin _xrOrigin;

        [Inject] private IPlayerLocation _playerLocation;
        
        public Transform CameraRigTransform => _xrOrigin.Camera.transform;

        private void Start()
        {
            _playerLocation.Init(this);
        }
    }
}
