using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    public class GunController : MonoSingleton<GunController>
    {
        public ControllerController CC;
        public GameObject GunOffset;

        public Vector3 ArmIKOffset = new Vector3(.05f, .0525f, -.1765f);

        public override void Awake()
        {
            base.Awake();
            CC = gameObject.GetComponent<ControllerController>();
            GunOffset = CC.GunOffset;
        }

        public void Update()
        {
            CC.ArmOffset.transform.localPosition = ArmIKOffset;
            SetControllers();
        }

        private void SetControllers()
        {
            if (Vars.GunControlCheck) CC.RenderModel.SetActive(true);
            else CC.RenderModel.SetActive(false);
        }
    }
}
