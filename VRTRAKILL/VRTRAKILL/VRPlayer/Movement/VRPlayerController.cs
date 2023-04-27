using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Movement
{
    internal class VRPlayerController : MonoBehaviour
    {
        CapsuleCollider CC;
        private void Start()
        {
            // Update player's height
            CC = GetComponent<CapsuleCollider>();
        }

        private void Update()
        {
            float DistanceFromFloor = Vector3.Dot(Vars.VRCameraContainer.transform.localPosition, Vector3.up);

            CC.height = Mathf.Max(CC.radius, DistanceFromFloor);
            //CC.center = Vars.VRCameraContainer.transform.localPosition - 0.5f * DistanceFromFloor * Vector3.up;
        }
    }
}