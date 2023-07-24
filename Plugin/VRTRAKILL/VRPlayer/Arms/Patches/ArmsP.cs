using HarmonyLib;
using Plugin.VRTRAKILL.VRPlayer.VRIK.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal class ArmsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Punch), nameof(Punch.Start))] static void ConvertArms(Punch __instance)
        {
            Arm A = null;
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            switch (__instance.type)
            {
                case FistType.Standard:
                    A = Arm.FeedbackerPreset(__instance.transform);
                    Vars.FeedbackerArm = A; break;
                case FistType.Heavy:
                    A = Arm.KnuckleblasterPreset(__instance.transform);
                    Vars.KnuckleblasterArm = A; break;
                case FistType.Spear:
                default:
                    break; // wtf is this?
            }
            AR.Arm = A; VRAC.Arm = A;
        }
    }
}
