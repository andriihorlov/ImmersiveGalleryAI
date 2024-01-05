using Unity.XR.CoreUtils;
using UnityEngine;
using Zenject;

namespace ImmersiveGalleryAI.Common.User
{
    public class UserData : MonoBehaviour
    {
        [SerializeField] private XROrigin _xrOrigin;

        [Inject] private IUser _user;
        
        public Transform CameraRigTransform => _xrOrigin.Camera.transform;

        private void Start()
        {
            _user.Init(this);
        }
    }
}
