using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.Input;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons;

[HarmonyPatch(typeof(WeaponWheel))] internal static class PatchWeaponWheel
{
    // Makes weapon wheel act using your controller's joystick input.
    [HarmonyPrefix] [HarmonyPatch(nameof(WeaponWheel.Update))]
    static bool Update(WeaponWheel __instance)
    {
        if (!GunControl.Instance
        || !GunControl.Instance.activated
        || OptionsManager.Instance.paused
        || NewMovement.Instance.dead
        || GameStateManager.Instance.PlayerInputLocked)
            __instance.gameObject.SetActive(value: false);

        else if (InputManager.Instance.InputSource.NextWeapon.WasCanceledThisFrame
             || InputManager.Instance.InputSource.PrevWeapon.WasCanceledThisFrame
             || InputManager.Instance.InputSource.LastWeapon.WasCanceledThisFrame
             || InputManager.Instance.InputSource.PreviousVariation.WasCanceledThisFrame)
        {
            if (__instance.selectedSegment != -1)
            {
                int target = __instance.segments[__instance.selectedSegment].slotIndex + 1;
                GunControl.Instance.SwitchWeapon(target);
            }

            __instance.gameObject.SetActive(value: false);
        }
        else
        {
            if (__instance.segments == null || __instance.segments.Count == 0)
                return false;

            __instance.direction = Vector2.ClampMagnitude(__instance.direction + SteamVRPlayerInput.MoveVector, 1f);
            float num = Mathf.Repeat(Mathf.Atan2(__instance.direction.x, __instance.direction.y) * 57.29578f + 90f, 360f); // the magic number of holy shit
            if (Mathf.Approximately(num, 360f))
                num = 0f;

            __instance.selectedSegment = ((__instance.direction.sqrMagnitude > 0f) ? ((int)(num / (360f / (float)__instance.segmentCount))) : __instance.selectedSegment);
            for (int i = 0; i < __instance.segments.Count; i++)
            {
                if (i == __instance.selectedSegment)
                    __instance.segments[i].SetActive(active: true);
                else __instance.segments[i].SetActive(active: false);
            }

            if (__instance.selectedSegment != __instance.lastSelectedSegment)
            {
                Object.Instantiate(__instance.clickSound);
                __instance.lastSelectedSegment = __instance.selectedSegment;
                if ((bool)RumbleManager.Instance)
                    RumbleManager.Instance.SetVibration(RumbleProperties.WeaponWheelTick);
            }
        }
        return false;
    }
}