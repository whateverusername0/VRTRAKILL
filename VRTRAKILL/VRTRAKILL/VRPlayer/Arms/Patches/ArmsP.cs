using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal class ArmsP
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FistControl), nameof(FistControl.Start))]
        [HarmonyPatch(typeof(FistControl), nameof(FistControl.ResetFists))]
        static void ConvertArms(FistControl __instance)
        {
            //__instance.transform.localPosition = new Vector3(0, 0, .25f);
            Helpers.Misc.RecursiveChangeLayer(__instance.gameObject, 0);
        }

        [HarmonyPostfix] [HarmonyPatch(typeof(Punch), nameof(Punch.Start))] static void ConvertArms(Punch __instance)
        {
            Armature A; ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            switch (__instance.type)
            {
                case FistType.Standard:
                    A = new Armature(__instance.transform, ArmType.Feedbacker);
                    AR.Arm = A; VRAC.Arm = A;
                    break;
                case FistType.Heavy:
                    A = new Armature(__instance.transform, ArmType.Knuckleblaster);
                    AR.Arm = A; VRAC.Arm = A;
                    break;
                case FistType.Spear:
                    break; // wtf is this?
            }
        }

        [HarmonyPostfix] [HarmonyPatch(typeof(HookArm), nameof(HookArm.Start))] static void ConvertWhiplash(HookArm __instance)
        {
            Armature A = new Armature(__instance.transform, ArmType.Whiplash);
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            AR.Arm = A; VRAC.Arm = A;
        }
    }
}
