using HarmonyLib;
using VRTRAKILL.Systems.Input;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Patches.ULTRAKILL.Movement;

[HarmonyPatch(typeof(NewMovement))] internal static class PatchNewMovement
{
    [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Respawn))]
    private static void Respawn(NewMovement __instance)
    {
        __instance.cc.enabled = false; // hi!
    }

    [HarmonyPostfix, HarmonyPatch(typeof(NewMovement), nameof(NewMovement.GetHurt))]
    public static void GetHurt(NewMovement __instance)
    {
        if (__instance.dead)
        {
            __instance.rb.constraints = __instance.defaultRBConstraints;
            __instance.cc.enabled = true;
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(NewMovement), nameof(NewMovement.Start))]
    static void StartPostfix(NewMovement __instance)
    {
        var vrcc = __instance.gameObject.EnsureComponent<VRCharacterController>();
        vrcc.TurnOffset = 0;
    }
}