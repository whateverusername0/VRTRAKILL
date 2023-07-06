using HarmonyLib;
using Plugin.VRTRAKILL.VRPlayer.VRIK.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal class ArmsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Punch), nameof(Punch.Start))] static void ConvertArms(Punch __instance)
        {
            Armature A = null;
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            Gestures.GesturesController GC = __instance.gameObject.AddComponent<Gestures.GesturesController>();
            switch (__instance.type)
            {
                case FistType.Standard:
                    A = Armature.FeedbackerPreset(__instance.transform);
                    Vars.FeedbackerArm = A; break;
                case FistType.Heavy:
                    A = Armature.KnuckleblasterPreset(__instance.transform);
                    Vars.KnuckleblasterArm = A; break;
                case FistType.Spear:
                default:
                    break; // wtf is this?
            }
            AR.Arm = A; VRAC.Arm = A; GC.Arm = A;
        }
    }
}
