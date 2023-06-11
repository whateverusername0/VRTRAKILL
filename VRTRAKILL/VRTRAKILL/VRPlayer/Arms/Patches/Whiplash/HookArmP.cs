using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches.Whiplash
{
    // you get it? cameracontrollerpatches?? ccp????? lol!!
    [HarmonyPatch] internal class HookArmP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(HookArm), nameof(HookArm.Start))] static void ConvertWhiplash(HookArm __instance)
        {
            Armature A = new Armature(__instance.transform, ArmType.Whiplash);
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            AR.Arm = A; VRAC.Arm = A;
        }
    }
}
