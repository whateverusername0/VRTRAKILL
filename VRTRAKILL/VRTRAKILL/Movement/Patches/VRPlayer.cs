using UnityEngine;
using HarmonyLib;

namespace Plugin.VRTRAKILL.Movement.Patches
{
    [HarmonyPatch] internal class VRPlayer
    {
        // for some reason when in VR the player is the double of it's size so i need to fix that
        // placeholder
        static float PlayerScaleMultiplier = 0.75f;

        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "Start")] static void ShirnkPlayer(NewMovement __instance)
        {
            __instance.gameObject.transform.localScale *= PlayerScaleMultiplier;
        }
    }
}
