using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGalleryAI.Common.Loader
{
    public class LoaderHandler : MonoBehaviour
    {
        [SerializeField] private Image _loaderImage;

        private void Awake()
        {
            SetActive(false);
        }

        public void SetActive(bool isActive)
        {
            _loaderImage.gameObject.SetActive(isActive);
            StopCoroutine(nameof(LoopLoading));

            if (isActive)
            {
                StartCoroutine(nameof(LoopLoading));
            }
        }

        private IEnumerator LoopLoading()
        {
            // Disabling by StopCoroutine()
            while (true)
            {
                
                _loaderImage.fillAmount += Time.deltaTime;
                if (_loaderImage.fillAmount > 0.98f)
                {
                    _loaderImage.fillAmount = 0f;
                }

                yield return null;
            }
        }
    }
}