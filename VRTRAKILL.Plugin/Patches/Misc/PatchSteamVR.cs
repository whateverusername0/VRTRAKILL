using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace VRTRAKILL.Patches.Misc;

[HarmonyPatch] internal static class PatchSteamVR
{
    /// <summary>
    ///     PATCH: Culled Expand() method since it's useless and adds more headache.
    /// </summary>
    [HarmonyPrefix] [HarmonyPatch(typeof(SteamVR_Camera), nameof(SteamVR_Camera.Expand))]
    static bool SVRCExpand(SteamVR_Camera __instance, Transform ____ears, Transform ____head)
    {
        foreach (var aud in Object.FindObjectsOfType<AudioListener>())
            Object.DestroyImmediate(aud);

        var inst = __instance;
        inst.gameObject.AddComponent<AudioListener>();
        inst.gameObject.AddComponent<SteamVR_Ears>();


        ____ears = __instance.transform;
        ____head = __instance.transform;

        return false;
    }
}
