using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Data;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(Shotgun))] internal static class PatchShotgun
{
    [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))]
    static void Transform(Shotgun __instance)
    {
        WeaponTransformHelper.ApplyTransform(ref __instance.wpos, new(-.02f, .2f, .26f), new(), new(.1f, .1f, .1f));

        // add our own hand until hakita decides otherwise.
        Transform Hand = Object.Instantiate(Assets.HandPose_Shotgun.transform);
        // Shotgun ******(Clone)/ShogunNewAnims/GunArmature/MainBone
        Hand.SetParent(__instance.transform.GetChild(2).GetChild(0).GetChild(0), false);
        Hand.localPosition = Vector3.zero;
        Hand.localEulerAngles = new Vector3(0, 0, 270);
        Hand.localScale = new Vector3(.1f, .1f, .1f);

        Hand.GetChild(1).GetChild(0).localPosition = new(-.5f, -.95f, -.45f);
        Hand.GetChild(1).GetChild(0).localEulerAngles = new(0, 180, 0);
        Hand.GetChild(1).GetChild(0).localScale = new(1500, 1500, 1500);
    }

    // TODO
}