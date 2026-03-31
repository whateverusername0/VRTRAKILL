using HarmonyLib;
using VRTRAKILL.Data;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(Chainsaw))] internal static class PatchChainsaw
{
    [HarmonyPostfix] [HarmonyPatch(nameof(Chainsaw.Start))]
    static void Transform(Chainsaw __instance)
    {
        __instance.lineStartTransform = GunControl.Instance?.currentWeapon != null
            ? GunControl.Instance?.currentWeapon.transform
            : GlobalVars.DominantHand.transform;
    }

    // TODO
}
