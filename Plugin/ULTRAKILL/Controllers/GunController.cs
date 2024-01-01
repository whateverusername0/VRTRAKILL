using VRTRAKILL.Utilities;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.VRPlayer.Controllers
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
            RM = CC.RenderModel.transform.GetChild(0).gameObject;
            SandboxHandRM = CC.RenderModel.transform.GetChild(1).gameObject;
            RM.SetActive(false);
            SandboxHandRM.SetActive(false);
        }

        public void Update()
        {
            CC.ArmOffset.transform.localPosition = ArmIKOffset;
            SetControllers();
        }

        private void SetControllers()
        {
            if ((bool)!GunControl.Instance?.activated || (bool)GunControl.Instance?.noWeapons)
            {
                if (GunControl.Instance != null
                && GunControl.Instance.currentWeapon != null
                && GunControl.Instance.currentWeapon.HasComponent<Sandbox.Arm.SandboxArm>()
                && !Vars.IsMainMenu)
                {
                    RM.SetActive(false);
                    SandboxHandRM.SetActive(true);
                }
                else
                {
                    RM.SetActive(true);
                    SandboxHandRM.SetActive(false);
                }
            }
            else
            {
                RM.SetActive(false);
                SandboxHandRM.SetActive(false);
            }
        }
    }
}
