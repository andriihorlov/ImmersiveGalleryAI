using UnityEngine;

namespace ImmersiveGalleryAI.Common.User
{
    public interface IUser 
    {
        Transform CameraRigTransform { get; }
        void Init(UserData userData);
    }
}
