using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class LeftArmController : MonoSingleton<LeftArmController>
    {
        public Vector3 Position;
        public Quaternion Rotation;

        private void Update()
        {
            Position = transform.position; Rotation = transform.rotation;
        }
    }
}
