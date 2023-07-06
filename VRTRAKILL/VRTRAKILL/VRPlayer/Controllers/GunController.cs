using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class GunController : MonoSingleton<GunController>
    {
        public GameObject GunOffset, UIOffset;

        public void Start()
        {
            GunOffset = gameObject.GetComponent<ControllerController>().GunOffset;
            UIOffset = gameObject.GetComponent<ControllerController>().UIOffset;
        }
    }
}
