using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class RightArmController : MonoSingleton<RightArmController>
    {
        public Vector3 Position;
        public Quaternion Rotation;

        private void Update()
        {
            Position = transform.position; Rotation = transform.rotation;
        }
    }
}
