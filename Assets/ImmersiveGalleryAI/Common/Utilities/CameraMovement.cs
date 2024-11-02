using System;
using DG.Tweening;
using UnityEngine;

namespace ImmersiveGalleryAI.Common.Utilities
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private CameraPath[] _path;

        [Space] [SerializeField] private int _indexPath;
        [SerializeField] private float _duration;
        [SerializeField] private float _rotationDuration = 2f;
        [SerializeField] private KeyCode _mainKey = KeyCode.Space;

        private bool _isPlaying;

        private void LateUpdate()
        {
            CheckIsNumberButtonClicked();
            if (!Input.GetKeyDown(_mainKey))
            {
                return;
            }

            _isPlaying = !_isPlaying;
            if (_isPlaying)
            {
                PlayAnimation();
            }
            else
            {
                StopAnimation();
            }
        }

        private void CheckIsNumberButtonClicked()
        {
            if (!Input.anyKeyDown)
            {
                return;
            }

            for (int i = 0; i < 10; i++)
            {
                string keyCode = "Alpha" + i;
                bool isButton = Enum.TryParse<KeyCode>(keyCode, out KeyCode key);
                if (!isButton)
                {
                    continue;
                }

                if (!Input.GetKeyDown(key))
                {
                    continue;
                }

                StopAnimation();
                _indexPath = i;
                PlayAnimation();
            }
        }

        private void PlayAnimation()
        {
            StopAnimation();
            CameraPath currentCameraPath = _path[_indexPath];
            
            Vector3[] positionPath = currentCameraPath.GetVectorPath();
            Quaternion[] rotation = currentCameraPath.GetPathRotation();

            _cameraTransform.position = positionPath[0];
            _cameraTransform.rotation = rotation[0];
            
            _cameraTransform
                .DOPath(positionPath, _duration, PathType.CatmullRom)
                .SetOptions(false)
                .SetUpdate(true)
                .OnWaypointChange(index =>
                {
                    if (currentCameraPath.IsRotated && index < rotation.Length)
                    {
                        _cameraTransform
                            .DORotateQuaternion(rotation[index], _duration / positionPath.Length)
                            .SetEase(Ease.InOutSine);
                    }
                })
                .SetUpdate(true)
                .SetId(_cameraTransform);
        }

        private void StopAnimation()
        {
            DOTween.Kill(_cameraTransform);
        }
    }
}