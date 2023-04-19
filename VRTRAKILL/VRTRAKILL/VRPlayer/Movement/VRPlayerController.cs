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

        private void Update()
        {
            // Update player's height
            float DistanceFromFloor = Vector3.Dot(Vars.VRCameraContainer.transform.localPosition, Vector3.up);
            CC.height = Mathf.Max(CC.radius, DistanceFromFloor);

            CC.center = Vars.VRCameraContainer.transform.localPosition - 0.5f * DistanceFromFloor * Vector3.up;
        }
    }
}
