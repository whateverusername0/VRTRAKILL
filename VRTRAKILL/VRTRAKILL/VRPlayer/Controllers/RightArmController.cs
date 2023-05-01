using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class RightArmController : MonoSingleton<RightArmController>
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
