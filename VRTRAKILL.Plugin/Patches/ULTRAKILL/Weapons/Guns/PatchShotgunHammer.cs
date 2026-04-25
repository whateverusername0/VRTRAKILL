using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(ShotgunHammer))] internal static class PatchShotgunHammer
{
    [HarmonyPostfix] [HarmonyPatch(nameof(ShotgunHammer.OnEnable))]
    static void Transform(ShotgunHammer __instance)
    {
        WeaponTransformHelper.ApplyTransform(ref __instance.wpos, new(0, -.15f, .25f), new(), new(.45f, .45f, .45f));
        __instance.transform.GetChild(1).localRotation = Quaternion.Euler(0, 180, 0);
        __instance.gameObject.GetComponent<Animator>().enabled = false;
    }

    // TODO
}
