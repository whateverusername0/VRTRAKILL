using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Movement
{
    internal class VRPlayerController : MonoBehaviour
    {
        private void Start()
        {
            // Update player's height
            CapsuleCollider CC = GetComponent<CapsuleCollider>();
            
            float DistanceFromFloor = Vector3.Dot(Vars.VRCameraContainer.transform.localPosition, Vector3.up);

            CC.height = Mathf.Max(CC.radius, DistanceFromFloor);

            CC.center = Vars.VRCameraContainer.transform.localPosition - 0.5f * DistanceFromFloor * Vector3.up;
        }
    }
}