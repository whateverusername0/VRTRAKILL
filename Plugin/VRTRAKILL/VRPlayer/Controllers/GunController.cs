using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class GunController : MonoSingleton<GunController>
    {
        public ControllerController CC;
        public GameObject GunOffset;

        public void Start()
        {
            CC = gameObject.GetComponent<ControllerController>();
            GunOffset = CC.GunOffset;
        }

        public void Update() => CC.ArmOffset.transform.localPosition = new Vector3(.075f, .125f, -.35f);
    }
}
