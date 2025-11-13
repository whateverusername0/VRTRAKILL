using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace VRTRAKILL.Patches.Misc;

[HarmonyPatch] internal static class PatchSteamVR
{
    // since SteamVR_Camera is a tasty piece and I don't want to reinvent the wheel
    // by fucking around w/ namings and break the game halfway through, i made this.
    [HarmonyPrefix] [HarmonyPatch(typeof(SteamVR_Camera), nameof(SteamVR_Camera.Expand))]
    static bool SVRCExpand(SteamVR_Camera __instance, Transform ____ears, Transform ____head)
    {
        __instance.gameObject.AddComponent<SteamVR_Ears>();
        ____ears = __instance.transform;
        ____head = __instance.transform;

        return false;
    }
}
