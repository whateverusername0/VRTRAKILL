using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class RightArmController : MonoSingleton<RightArmController>
    {
        public GameObject Offset = new GameObject("Offset");

        public void Start()
        {
            Offset.transform.parent = this.transform;
            Offset.transform.localPosition = Vector3.zero;
            Offset.transform.localRotation = Quaternion.Euler(45, 0, 0);
        }

        public void Update()
        {
            if (Vars.Config.VRSettings.DrawControllerLines)
            {
                Debug.DrawLine(Offset.transform.position,
                               Offset.transform.position + Offset.transform.forward * 100,
                               Color.white, 0, false);
            }
        }
    }
}
