using UnityEngine;

namespace ImmersiveGalleryAI.Common.Utilities
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _targetTransform;
        private Transform _currentTransform;

        private void Awake()
        {
            _currentTransform = transform;
        }

        private void LateUpdate()
        {
            _currentTransform.position = _targetTransform.position;
        }
    }
}
