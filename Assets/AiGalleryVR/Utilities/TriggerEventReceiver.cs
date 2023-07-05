using System;
using UnityEngine;

namespace AiGalleryVR.Utilities
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TriggerEventReceiver : MonoBehaviour
    {
        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerExit;

        private void OnTriggerEnter(Collider otherCollider)
        {
            TriggerEnter?.Invoke(otherCollider);
        }

        private void OnTriggerExit(Collider otherCollider)
        {
            TriggerExit?.Invoke(otherCollider);
        }
    }
}