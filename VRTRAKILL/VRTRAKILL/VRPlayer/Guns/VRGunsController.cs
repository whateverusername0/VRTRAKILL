using Plugin.VRTRAKILL.VRPlayer.Controllers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns
{
    internal class VRGunsController : MonoBehaviour
    {
        private Vector3 Offset;

        private Vector3 VMScale = new Vector3(0.5f, 0.5f, 0.5f);
        private Vector3 TargetPos = Vector3.zero;
        private float
            VMFwd = 0.35f,
            VMShift = 0.1f,
            VMHeight = -0.1f;

        private void Start()
        {
            GetComponent<WalkingBob>().enabled = false; // disable weapon bobbing when moving
            transform.localScale = VMScale;
            Offset = transform.localPosition;
            TargetPos = transform.position;
        }

        private void Update()
        {
            transform.position = RightArmController.Instance.Position;
            transform.rotation = RightArmController.Instance.Rotation;
        }
    }
}
