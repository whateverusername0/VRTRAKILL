using HarmonyLib;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(RocketLauncher))] internal static class PatchRocketLauncher
{
    [HarmonyPostfix] [HarmonyPatch(nameof(RocketLauncher.Start))]
    static void Transform(RocketLauncher __instance)
    {
        // TODO
    }

    // TODO
}
