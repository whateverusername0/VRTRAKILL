using Plugin.Util;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    public class GunController : MonoSingleton<GunController>
    {
        public ControllerController CC;
        public GameObject GunOffset;
        public GameObject RM, SandboxHandRM;

        public Vector3 ArmIKOffset = new Vector3(.05f, .0525f, -.1765f);

        public override void Awake()
        {
            base.Awake();
            CC = gameObject.GetComponent<ControllerController>();
            GunOffset = CC.GunOffset;
        }
        public void Start()
        {
            RM = CC.RenderModel;
        }

        public void Update()
        {
            CC.ArmOffset.transform.localPosition = ArmIKOffset;
            SetControllers();
        }

        private void SetControllers()
        {
            if (GunControl.Instance != null
            && GunControl.Instance.currentWeapon != null
            && GunControl.Instance.currentWeapon.HasComponent<Sandbox.Arm.SandboxArm>())
                CC.RenderModel = SandboxHandRM;
            else CC.RenderModel = RM;

            if (!((bool)GunControl.Instance?.activated && (bool)GunControl.Instance?.noWeapons)
            || ((bool)GunControl.Instance?.noWeapons && !Vars.IsPlayerFrozen))
                CC.RenderModel.SetActive(true);
            else CC.RenderModel.SetActive(false);
        }
    }
}
