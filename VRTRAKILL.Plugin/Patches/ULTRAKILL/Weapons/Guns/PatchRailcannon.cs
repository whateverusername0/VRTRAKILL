using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Data;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(Railcannon))] internal static class PatchRailcannon
{
    [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))]
    static void Transform(Railcannon __instance)
    {
        __instance.wpos.defaultPos += new Vector3(-.15f, .15f, -.175f);
        __instance.wpos.defaultScale = new(.25f, .25f, .25f);

        // add our own hand until hakita decides otherwise.
        Transform Hand = Object.Instantiate(Assets.HandPose_Railgun.transform);
        // Railcannon ******(Clone)/Railgun/Armature/Base
        Hand.SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);
        Hand.localPosition = Vector3.zero;

        Hand.GetChild(1).GetChild(0).localPosition = new(-.1f, -.325f, -.025f);
        Hand.GetChild(1).GetChild(0).localEulerAngles = new(30, 180, 0);
        Hand.GetChild(1).GetChild(0).localScale = new(350, 350, 350);
    }

    // TODO
}