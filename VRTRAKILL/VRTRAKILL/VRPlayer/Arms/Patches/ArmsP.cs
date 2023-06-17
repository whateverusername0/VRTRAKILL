using HarmonyLib;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal class ArmsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Punch), nameof(Punch.Start))] static void ConvertArms(Punch __instance)
        {
            Armature A;
            ArmRemover AR = __instance.gameObject.AddComponent<ArmRemover>();
            VRArmsController VRAC = __instance.gameObject.AddComponent<VRArmsController>();
            Gestures.GesturesController GC = __instance.gameObject.AddComponent<Gestures.GesturesController>();
            VRIK.VRIKController VRIKC = __instance.gameObject.AddComponent<VRIK.VRIKController>();
            switch (__instance.type)
            {
                case FistType.Standard:
                    A = new Armature(__instance.transform, ArmType.Feedbacker);
                    AR.Arm = A; VRAC.Arm = A; GC.Arm = A; VRIKC.Arm = A;
                    break;
                case FistType.Heavy:
                    A = new Armature(__instance.transform, ArmType.Knuckleblaster);
                    AR.Arm = A; VRAC.Arm = A; GC.Arm = A; VRIKC.Arm = A;
                    break;
                case FistType.Spear:
                    break; // wtf is this?
            }
        }
    }
}
