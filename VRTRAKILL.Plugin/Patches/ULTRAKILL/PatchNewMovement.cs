using HarmonyLib;

namespace VRTRAKILL.Patches.ULTRAKILL.Movement;

[HarmonyPatch(typeof(NewMovement))] internal static partial class PatchNewMovement
{
    // TODO

    [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Respawn))]
    static void Respawn(NewMovement __instance)
    {
        __instance.cc.enabled = false; // hi! fuck you
    }
}