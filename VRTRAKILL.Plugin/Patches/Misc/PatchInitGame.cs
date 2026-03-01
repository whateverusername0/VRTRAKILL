using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems.VRCamera;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Patches.Misc;

[HarmonyPatch(typeof(InitGame))] internal class PatchInitGame
{
    [HarmonyPostfix] [HarmonyPatch(nameof(InitGame.Awake))] static void Awake()
    {
        var mainCam = Camera.main;
        mainCam.gameObject.EnsureComponent<SteamVRBridge>();
    }
}