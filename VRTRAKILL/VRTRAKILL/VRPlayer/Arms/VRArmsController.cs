using Plugin.VRTRAKILL.VRPlayer.Controllers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        private Vector3 Offset;

        private Vector3 VMScale = new Vector3(0.3f, 0.3f, 0.3f);
        private Vector3 TargetPos = Vector3.zero;
        private float
            VMFwd = 0.35f,
            VMShift = 0.1f,
            VMHeight = -0.1f;

        private void Start()
        {
            transform.localScale = VMScale;
            Offset = transform.localPosition;
            TargetPos = transform.position;
        }

        private void Update()
        {
            transform.position = LeftArmController.Instance.Position;
            transform.rotation = LeftArmController.Instance.Rotation;
        }
    }
}
