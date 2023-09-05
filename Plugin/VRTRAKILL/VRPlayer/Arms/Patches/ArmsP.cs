using HarmonyLib;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal sealed class ArmsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Punch), nameof(Punch.Start))] static void ConvertArms(Punch __instance)
        {
            Arm A = null;
            ArmTransformer AR = __instance.gameObject.AddComponent<ArmTransformer>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            switch (__instance.type)
            {
                case FistType.Standard: A = Arm.FeedbackerPreset(__instance.transform); break;
                case FistType.Heavy: A = Arm.KnuckleblasterPreset(__instance.transform); break;
                case FistType.Spear:
                default: break;
            }
            AR.Arm = A; VRAC.Arm = A;
        }
    }
}
