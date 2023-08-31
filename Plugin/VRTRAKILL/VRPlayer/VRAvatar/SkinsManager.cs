using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
{
    //[RequireComponent(typeof(VRigController))]
    //internal class SkinsManager : MonoBehaviour
    //{
    //    VRigController Rigger;
    //    SkinnedMeshRenderer SMR_Body;

    //    public void Awake()
    //    {
    //        if (Vars.Config.VRBody.EnableSkins) Destroy(this);

    //        Rigger = VRigController.Instance;
    //        SMR_Body = Rigger.Rig.GameObjectT.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>();

    //        if (Vars.Config.VRBody.Skins.V1) ApplyV1Skin();
    //        else if (Vars.Config.VRBody.Skins.V2) ApplyV2Skin();
    //        else ApplyV1Skin();
    //    }

    //    // Element 0 - Body, 1-9 - Wings
    //    private void ApplyV1Skin()
    //    {
    //        SMR_Body.materials[0] = Assets.Vars.Skin_V1[0];
    //        for (int i = 1; i < SMR_Body.materials.Length; i++)
    //            SMR_Body.materials[i] = Assets.Vars.Skin_V1[1];

    //        if (Vars.Config.Controllers.LeftHanded)
    //            Rigger.Rig._RFeedbacker.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().material = Assets.Vars.Skin_V1[2];
    //        else
    //            Rigger.Rig._LFeedbacker.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().material = Assets.Vars.Skin_V1[2];
    //    }
    //    private void ApplyV2Skin()
    //    {
    //        SMR_Body.materials[0] = Assets.Vars.Skin_V2[0];
    //        for (int i = 1; i < SMR_Body.materials.Length; i++)
    //            SMR_Body.materials[i] = Assets.Vars.Skin_V2[1];

    //        if (Vars.Config.Controllers.LeftHanded)
    //            Rigger.Rig._RFeedbacker.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().material = Assets.Vars.Skin_V2[2];
    //        else
    //            Rigger.Rig._LFeedbacker.GameObjecT.GetComponentInChildren<SkinnedMeshRenderer>().material = Assets.Vars.Skin_V2[2];
    //    }
    //}
}
