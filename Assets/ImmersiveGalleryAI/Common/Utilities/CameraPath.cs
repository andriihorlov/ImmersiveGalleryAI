using UnityEngine;

namespace ImmersiveGalleryAI.Common.Utilities
{
    public class CameraPath : MonoBehaviour
    {
        [SerializeField] private Transform[] _path;
        [SerializeField] private bool _isRotated;

        public bool IsRotated => _isRotated;

        public Vector3[] GetVectorPath()
        {
            Vector3[] path = new Vector3[_path.Length];
            for (int index = 0; index < _path.Length; index++)
            {
                path[index] = _path[index].position;
            }

            return path;
        }

        public Quaternion[] GetPathRotation()
        {
            Quaternion[] rotation = new Quaternion[_path.Length];
            for (int index = 0; index < _path.Length; index++)
            {
                rotation[index] = _path[index].rotation;
            }

            return rotation;
        }

        public Quaternion GetTargetRotation(int index) => _path[index].rotation;
    }
}