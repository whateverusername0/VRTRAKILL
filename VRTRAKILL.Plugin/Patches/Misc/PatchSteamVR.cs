using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace VRTRAKILL.Patches.Misc;

[HarmonyPatch] internal static class PatchSteamVR
{
    /// <summary>
    ///     PATCH: Tweaked Expand() method, making it provide less headache.
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

        if (__instance.transform.parent == null)
        {
            var parent = new GameObject($"{__instance.gameObject.name}_VROrigin");

            // double checking the original position.
            parent.transform.position = __instance.transform.position;
            parent.transform.rotation = __instance.transform.rotation;
            parent.transform.localScale = __instance.transform.localScale;

            // triple checking. god save.
            __instance.transform.SetParent(parent.transform, true);
        }

        return false;
    }
}
