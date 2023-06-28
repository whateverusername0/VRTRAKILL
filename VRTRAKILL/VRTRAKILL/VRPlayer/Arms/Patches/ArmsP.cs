using HarmonyLib;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal class ArmsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Punch), nameof(Punch.Start))] static void ConvertArms(Punch __instance)
        {
            VRIK.Armature A = null;
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            Gestures.GesturesController GC = __instance.gameObject.AddComponent<Gestures.GesturesController>();
            switch (__instance.type)
            {
                case FistType.Standard:
                    A = VRIK.Armature.FeedbackerPreset(__instance.transform);
                    Vars.FeedbackerArm = A; break;
                case FistType.Heavy:
                    A = VRIK.Armature.KnuckleblasterPreset(__instance.transform);
                    Vars.KnuckleblasterArm = A; break;
                case FistType.Spear:
                default:
                    break; // wtf is this?
            }
            AR.Arm = A; VRAC.Arm = A; GC.Arm = A;
        }
    }
}
