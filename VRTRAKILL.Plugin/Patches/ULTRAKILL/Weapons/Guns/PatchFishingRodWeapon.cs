using HarmonyLib;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(FishingRodWeapon))] internal static class PatchFishingRodWeapon
{
    [HarmonyPostfix] [HarmonyPatch(nameof(FishingRodWeapon.Awake))]
    static void Transform(FishingRodWeapon __instance)
    {
        WeaponTransformHelper.ApplyTransform(__instance.GetComponent<WeaponPos>(), new(.05f, -.06f, -.185f), new(10, 90, 20), new(-.0023f, .0023f, .0023f));
    }
}
