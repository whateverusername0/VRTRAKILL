using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace Plugin.Util.Patches
{
    [HarmonyPatch] internal static class SteamVRP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(SteamVR_Camera), nameof(SteamVR_Camera.Expand))]
        static bool SVRCExpand(SteamVR_Camera __instance, Transform ____ears, Transform ____head)
        {
            // I can't believe i'm patching a whole ass svr method to fit my needs. oh well
            __instance.gameObject.AddComponent<SteamVR_Ears>();
            ____ears = __instance.transform;

            //__instance.gameObject.AddComponent<SteamVR_TrackedObject>();
            ____head = __instance.transform;

            return false;
        }
    }
}
