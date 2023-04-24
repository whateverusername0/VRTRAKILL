using Plugin.VRTRAKILL.VRPlayer.Controllers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns
{
    internal class VRGunsController : MonoBehaviour
    {
        private Vector3 VMScale = new Vector3(0.4f, 0.4f, 0.4f);

        private void Start()
        {
            GetComponent<WalkingBob>().enabled = false; // disable weapon bobbing when moving
            transform.localScale = VMScale;
        }

        private void Update()
        {
            transform.position = RightArmController.Instance.Position;
            transform.rotation = RightArmController.Instance.Rotation;
        }
    }
}
