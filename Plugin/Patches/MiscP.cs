using HarmonyLib;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Patches
{
    // contains necessary patches (and not so necessary)
    [HarmonyPatch] internal sealed class MiscP
    {
        // discord :)
        [HarmonyPrefix] [HarmonyPatch(typeof(DiscordController), nameof(DiscordController.SendActivity))]
        static void SendActivity(DiscordController __instance)
        => __instance.cachedActivity.State = "Playing in VR via VRTRAKILL";

        // weapon wheel controller interaction
        [HarmonyPrefix] [HarmonyPatch(typeof(WeaponWheel), nameof(WeaponWheel.Update))] static bool WWUpdate(WeaponWheel __instance)
        {
            if (!MonoSingleton<GunControl>.Instance
                || !MonoSingleton<GunControl>.Instance.activated
                || MonoSingleton<OptionsManager>.Instance.paused
                || MonoSingleton<NewMovement>.Instance.dead
                || GameStateManager.Instance.PlayerInputLocked)
            {
                __instance.gameObject.SetActive(value: false);
                return false;
            }

            __instance.direction = Vector2.ClampMagnitude(__instance.direction + Input.InputVars.WWVector, 1f);
            float num = Mathf.Repeat(Mathf.Atan2(__instance.direction.x, __instance.direction.y) * 57.29578f + 90f, 360f); // wtf is that magic number??
            __instance.selectedSegment = ((__instance.direction.sqrMagnitude > 0f)
                                         ? ((int)(num / (360f / (float)__instance.segmentCount)))
                                         : __instance.selectedSegment);

            if (MonoSingleton<InputManager>.Instance.InputSource.NextWeapon.WasCanceledThisFrame
                || MonoSingleton<InputManager>.Instance.InputSource.PrevWeapon.WasCanceledThisFrame
                || MonoSingleton<InputManager>.Instance.InputSource.LastWeapon.WasCanceledThisFrame)
            {
                if (__instance.selectedSegment != -1)
                    MonoSingleton<GunControl>.Instance.SwitchWeapon(__instance.selectedSegment + 1);

                __instance.gameObject.SetActive(value: false);
                return false;
            }

            for (int i = 0; i < __instance.segments.Count; i++)
            {
                if (i == __instance.selectedSegment) __instance.segments[i].SetActive(active: true);
                else __instance.segments[i].SetActive(active: false);
            }

            if (__instance.selectedSegment != __instance.lastSelectedSegment)
            {
                Object.Instantiate(__instance.clickSound);
                __instance.lastSelectedSegment = __instance.selectedSegment;
                if ((bool)MonoSingleton<RumbleManager>.Instance)
                    MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.WeaponWheelTick);
            }
            return false;
        }
    }
}
