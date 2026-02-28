using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace VRTRAKILL.Patches.Misc;

[HarmonyPatch(typeof(InitGame))] internal class PatchInitGame
{
    [HarmonyPostfix] [HarmonyPatch(nameof(InitGame.Awake))] static void Awake()
    {
        var mainCam = Camera.main;

        var r = mainCam.gameObject.AddComponent<SteamVR_Render>();

        var cam = mainCam.gameObject.AddComponent<SteamVR_Camera>();

        var tr = mainCam.gameObject.AddComponent<SteamVR_TrackedObject>();
        tr.index = SteamVR_TrackedObject.EIndex.Hmd;
    }
}