using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

namespace ImmersiveGalleryAI.Common.Utilities
{
    public class StarsMoving : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 1f;

        private XRController _xrController;
        private MeshRenderer _meshRenderer;
        private Transform _currentTransform;
        private Vector3 _rotationDirection;

        private bool _isSphereVisible = true;

        private void Awake()
        {
            _currentTransform = transform;
            _xrController = FindObjectOfType<XRController>();
            _meshRenderer = GetComponent<MeshRenderer>();
            int random = Random.Range(0, 99);

            if (random < 30)
            {
                _rotationDirection.x = 1f;
            }
            else if (random > 30 && random < 60)
            {
                _rotationDirection.y = 1f;
            }
            else
            {
                _rotationDirection.z = 1f;
            }
        }

        private void LateUpdate()
        {
            if (_xrController.inputDevice.IsPressed(InputHelpers.Button.Trigger, out bool isPressed))
            {
                ToggleMeshVisibility();
            }

            if (_meshRenderer.enabled)
            {
                _currentTransform.localEulerAngles += Time.deltaTime * _rotationDirection * _rotationSpeed;
            }
        }

        [ContextMenu("Simulate trigger press")]
        private void ToggleMeshVisibility()
        {
            _isSphereVisible = !_isSphereVisible;
            _meshRenderer.enabled = _isSphereVisible;
        }
    }
}