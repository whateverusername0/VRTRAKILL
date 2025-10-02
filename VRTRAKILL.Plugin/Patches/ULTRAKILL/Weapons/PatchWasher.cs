using HarmonyLib;
using Plugin.Data;
using UnityEngine;

namespace Plugin.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(Washer))] internal class PatchWasher
{
    [HarmonyPrefix] [HarmonyPatch(nameof(Washer.Update))]
    private static bool Update(Washer __instance)
    {
        Transform transform = Vars.DominantHand.transform;
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, 50f, LayerMaskDefaults.Get(LMD.Environment)))
        {
            if (hitInfo.distance < 2.25f)
            {
                __instance.transform.position = transform.position;
                __instance.transform.rotation = transform.rotation;
                __instance.correctCameraView.canModifyTarget = false;
            }
            else
            {
                __instance.correctCameraView.canModifyTarget = true;
            }
        }

        if (MonoSingleton<GunControl>.Instance.activated && !GameStateManager.Instance.PlayerInputLocked)
        {
            if (__instance.inputManager.InputSource.Fire1.IsPressed && !__instance.isSpraying)
            {
                __instance.StartWashing();
            }
            else if (!__instance.inputManager.InputSource.Fire1.IsPressed && __instance.isSpraying)
            {
                __instance.StopWashing();
            }

            if (__instance.inputManager.InputSource.Fire2.WasPerformedThisFrame)
            {
                __instance.SwitchNozzle();
            }
        }

        float f = (float)((double)Time.time % 6.283185);
        __instance.aud.pitch = ((__instance.nozzleMode == 2) ? 2.1f : 1.1f) + Mathf.Sin(f) * 0.025f;
        return false;
    }
}
