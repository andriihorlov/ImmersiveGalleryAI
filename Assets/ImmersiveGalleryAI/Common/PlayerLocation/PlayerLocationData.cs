using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Zenject;

namespace ImmersiveGalleryAI.Common.PlayerLocation
{
    public class PlayerLocationData : MonoBehaviour
    {
        [SerializeField] private XROrigin _xrOrigin;
        [SerializeField] private LocomotionProvider _locomotionProvider;

        [Inject] private IPlayerLocation _playerLocation;

        public Transform CameraRigTransform => _xrOrigin.Camera.transform;

        private void Start()
        {
            _playerLocation.Init(this);
        }

        private void OnEnable()
        {
            _locomotionProvider.endLocomotion += LocomotionProviderOnendLocomotion;
        }

        private void OnDisable()
        {
            _locomotionProvider.endLocomotion -= LocomotionProviderOnendLocomotion;
        }

        private void LocomotionProviderOnendLocomotion(LocomotionSystem obj)
        {
            _playerLocation.PlayerTeleported();
        }
    }
}