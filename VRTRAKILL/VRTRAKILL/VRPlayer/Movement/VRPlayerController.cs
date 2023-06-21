using UnityEngine;
using System.Collections;

namespace Plugin.VRTRAKILL.VRPlayer.Movement
{
    internal class VRPlayerController : MonoBehaviour
    {
        CapsuleCollider CC;
        public void Start()
        {
            CC = GetComponent<CapsuleCollider>();
        }
        public void FixedUpdate()
        {
            // Updates ingame player center to match irl player position
            float DistanceFromFloor = Vector3.Dot(Vars.VRCameraContainer.transform.localPosition, Vector3.up);
            CC.center = Vars.VRCameraContainer.transform.localPosition - 0.5f * DistanceFromFloor * Vector3.up;
        }
    }
}