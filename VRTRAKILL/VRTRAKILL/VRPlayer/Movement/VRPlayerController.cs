using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Movement
{
    internal class VRPlayerController : MonoBehaviour
    {
        public CapsuleCollider CC;

        private void Start()
        {
            CC = GetComponent<CapsuleCollider>();
        }

        private void FixedUpdate()
        {
            // Update player's height
            float DistanceFromFloor = Vector3.Dot(Vars.VRCameraContainer.transform.localPosition, Vector3.up);

            if (NewMovement.Instance.sliding) CC.height = Mathf.Max(CC.radius, DistanceFromFloor - 0.35f);
            else CC.height = Mathf.Max(CC.radius, DistanceFromFloor);

            CC.center = Vars.VRCameraContainer.transform.localPosition - 0.5f * DistanceFromFloor * Vector3.up;
        }
    }
}