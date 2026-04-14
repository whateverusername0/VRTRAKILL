using VRTRAKILL.Utilities;
using UnityEngine;
using VRTRAKILL.Data;

namespace VRTRAKILL.Systems.Controllers;

public class VRGunsSystem : MonoSingleton<VRGunsSystem>
{
    public VRControllersSystem CC;
    public Transform GunOffset;
    public GameObject RM, SandboxHandRM;

    public Vector3 ArmIKOffset = new Vector3(.05f, .0525f, -.1765f);

    public void Awake()
    {
        CC = gameObject.GetComponent<VRControllersSystem>();
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
            && !GlobalVars.IsMainMenu)
            {
                RM?.SetActive(false);
                SandboxHandRM?.SetActive(true);
            }
            else
            {
                RM?.SetActive(true);
                SandboxHandRM?.SetActive(false);
            }
        }
        else
        {
            RM?.SetActive(false);
            SandboxHandRM?.SetActive(false);
        }
    }
}
