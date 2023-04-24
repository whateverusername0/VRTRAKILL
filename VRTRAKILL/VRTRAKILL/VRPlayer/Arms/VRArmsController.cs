using Plugin.VRTRAKILL.VRPlayer.Controllers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms
{
    internal class VRArmsController : MonoBehaviour
    {
        private Vector3 VMScale = new Vector3(0.4f, 0.4f, 0.4f);

        private void Start()
        {
            transform.localScale = VMScale;
        }

        private void Update()
        {
            transform.position = LeftArmController.Instance.Position;
            transform.rotation = LeftArmController.Instance.Rotation;
        }
    }
}
