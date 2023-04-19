using HarmonyLib;
using System;

namespace Plugin.VRTRAKILL.VRPlayer.VRCamera.Patches
{
    [HarmonyPatch(typeof(NewMovement))] internal class FixCameraOnRespawn
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(NewMovement.Respawn))] static void SetPlayerRotation(NewMovement __instance)
        {
            // HOW :(
        }
    }
}
