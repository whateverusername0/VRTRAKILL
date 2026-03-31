using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems.VRCamera;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Patches.Misc;

[HarmonyPatch(typeof(InitGame))] internal class PatchInitGame
{
    /// <summary>
    ///     Wanted to see the intro cutscene in VR? Now you can!
    /// </summary>
    [HarmonyPostfix] [HarmonyPatch(nameof(InitGame.Awake))] static void Awake()
    {
        var mainCam = Camera.main;
        var ovrb = mainCam.gameObject.EnsureComponent<OpenXRBridge>();
        ovrb.RenderingCamera = mainCam;
    }
}