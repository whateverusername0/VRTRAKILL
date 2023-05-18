using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class GunController : MonoSingleton<GunController>
    {
        public GameObject Offset;

        public void Start()
        {
            Offset = gameObject.GetComponent<ControllerController>().Offset;
        }

        public void Update()
        {

        }
    }
}
