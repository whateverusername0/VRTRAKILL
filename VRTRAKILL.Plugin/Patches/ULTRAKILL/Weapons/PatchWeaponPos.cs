using HarmonyLib;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(WeaponPos))] internal static class PatchWeaponPos
{
    [HarmonyPrefix] [HarmonyPatch(nameof(WeaponPos.Start))]
    static void Start(WeaponPos __instance)
    {
        __instance.middlePos = __instance.defaultPos;
        __instance.middleRot = __instance.defaultRot;
        __instance.middleScale = __instance.defaultScale;
    }
}