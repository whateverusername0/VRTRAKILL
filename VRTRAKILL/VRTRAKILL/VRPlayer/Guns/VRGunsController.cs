using Plugin.VRTRAKILL.VRPlayer.Controllers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns
{
    internal class VRGunsController : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<WalkingBob>().enabled = false; // disable weapon bobbing when moving
        }

        private void Update()
        {
            transform.position = RightArmController.Instance.Position;
            transform.rotation = RightArmController.Instance.Rotation;
        }
    }
}
