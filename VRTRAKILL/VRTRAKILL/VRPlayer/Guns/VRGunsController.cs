using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns
{
    internal class VRGunsController : MonoBehaviour
    {
        public void Update()
        {
            transform.position = Vars.RightController.transform.position;
            transform.rotation = Vars.RightController.transform.rotation;
        }
    }
}
