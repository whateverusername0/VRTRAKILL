using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems;
using VRTRAKILL.Systems.Arms;
using VRTRAKILL.Systems.VRAvatar.Armature;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(FishingRodWeapon))] internal static class PatchFishingRodWeapon
{
    [HarmonyPostfix] [HarmonyPatch(nameof(FishingRodWeapon.Awake))]
    static void Transform(FishingRodWeapon __instance)
    {
        WeaponTransform.ApplyTransform(__instance.GetComponent<WeaponPos>(), new(.05f, -.06f, -.185f), new(10, 90, 20), new(-.0023f, .0023f, .0023f));

        VRWeaponArmController WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
        Arm A = Arm.FeedbackerPreset(__instance.transform);
        WAC.Arm = A; WAC.OffsetPos = Vector3.zero;
    }
}
