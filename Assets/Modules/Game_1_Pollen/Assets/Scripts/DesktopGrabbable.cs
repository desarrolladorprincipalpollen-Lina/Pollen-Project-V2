using UnityEngine;
using UnityEngine.InputSystem;

namespace PollenModule
{
    public class DesktopGrabbable : MonoBehaviour
    {
        private Transform originalParent;
        private Rigidbody rb;
        private bool isGrabbed = false;
        private Transform holdPoint;
        private float grabDistance = 2f;
        private float throwForce = 5f;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Grab(Transform holder)
        {
            if (isGrabbed) return;

            isGrabbed = true;
            holdPoint = holder;
            originalParent = transform.parent;
            
            // Disable physics while holding
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
            }

            // Parent to the holder (camera/hand)
            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void Release()
        {
            if (!isGrabbed) return;

            isGrabbed = false;
            transform.SetParent(originalParent);

            // Re-enable physics
            if (rb != null)
            {
                rb.useGravity = true;
                rb.isKinematic = false;
                // Add some forward force if needed
                // rb.AddForce(holdPoint.forward * throwForce, ForceMode.Impulse);
            }
            
            holdPoint = null;
        }

        void Update()
        {
            // Optional: Add logic here if you want to rotate the object while holding, etc.
        }
    }
}
