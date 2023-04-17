using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.Patches
{
    [HarmonyPatch] internal class CheckpointCameraFix
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(NewMovement), "DeactivatePlayer")] static bool DontDisableCCOnDeath
        (NewMovement __instance)
        {
            __instance.activated = false;
            // MonoSingleton<CameraController>.Instance.activated = true; // this should work (i wish)
            MonoSingleton<GunControl>.Instance.NoWeapon();
            MonoSingleton<FistControl>.Instance.NoFist();
            if (__instance.sliding) __instance.StopSlide();
            return false;
        }
    }
}
