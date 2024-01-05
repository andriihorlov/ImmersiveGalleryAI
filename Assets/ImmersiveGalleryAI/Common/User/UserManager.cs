using UnityEngine;

namespace ImmersiveGalleryAI.Common.User
{
    public class UserManager : IUser 
    {
        public Transform CameraRigTransform { get; private set; }

        public void Init(UserData userData)
        {
            CameraRigTransform = userData.CameraRigTransform;
        }
    }
}