using HarmonyLib;
using UnityEngine;

namespace VRTRAKILL.Patches.ULTRAKILL;

[HarmonyPatch(typeof(Punch))] internal static class PatchPunch
{
    public static Vector3 Direction;

    [HarmonyPostfix] [HarmonyPatch(nameof(Punch.Start))]
    static void Start(Punch __instance)
    {
        // inshallah pls stop
        foreach (SkinnedMeshRenderer SMR in __instance.GetComponentsInChildren<SkinnedMeshRenderer>())
            SMR.updateWhenOffscreen = true;
    }

    // TODO
}
