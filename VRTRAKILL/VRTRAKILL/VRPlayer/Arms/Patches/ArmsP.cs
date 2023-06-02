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
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            switch (__instance.type)
            {
                case FistType.Standard:
                    Vars.FeedbackerArmature = new Feedbacker.Armature(__instance.transform);
                    AR.FBArm = Vars.FeedbackerArmature;
                    Feedbacker.FeedbackerController FBC = __instance.gameObject.AddComponent<Feedbacker.FeedbackerController>();
                    FBC.Arm = Vars.FeedbackerArmature;
                    break;
                case FistType.Heavy:
                    Vars.KnuckleblasterArmature = new Knuckleblaster.Armature(__instance.transform);
                    AR.KBArm = Vars.KnuckleblasterArmature;
                    break;
                case FistType.Spear:
                    break; // wtf is this?
            }
        }

        [HarmonyPostfix] [HarmonyPatch(typeof(HookArm), nameof(HookArm.Start))] static void ConvertWhiplash(HookArm __instance)
        {
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();

        }
    }
}
