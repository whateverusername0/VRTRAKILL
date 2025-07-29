using HarmonyLib;
using UnityEngine;
using Plugin.Systems.Input;

namespace Plugin.Systems.Guns.Patches
{
    [HarmonyPatch] internal sealed class GunsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(GunControl), nameof(GunControl.Start))] static void RLPGC(GunControl __instance)
        {
            __instance.GetComponent<WalkingBob>().enabled = false;
            __instance.transform.localPosition = Vector3.zero;
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.ResetWeapons))] static void RLPGS(GunSetter __instance, bool firstTime = false)
        { __instance.transform.localPosition = Vector3.zero; }
        [HarmonyPostfix] [HarmonyPatch(typeof(GunSetter), nameof(GunSetter.Start))] static void MakeThemUndisappear(GunSetter __instance)
        {
            foreach (SkinnedMeshRenderer SMR in __instance.GetComponentsInChildren<SkinnedMeshRenderer>())
                SMR.updateWhenOffscreen = true;
        }

        // weapon wheel controller interaction
        [HarmonyPrefix] [HarmonyPatch(typeof(WeaponWheel), nameof(WeaponWheel.Update))] static bool WWUpdate(WeaponWheel __instance)
        {
            if (!MonoSingleton<GunControl>.Instance
            || !MonoSingleton<GunControl>.Instance.activated
            || MonoSingleton<OptionsManager>.Instance.paused
            || MonoSingleton<NewMovement>.Instance.dead
            || GameStateManager.Instance.PlayerInputLocked)
                __instance.gameObject.SetActive(value: false);

            else if (MonoSingleton<InputManager>.Instance.InputSource.NextWeapon.WasCanceledThisFrame
                 || MonoSingleton<InputManager>.Instance.InputSource.PrevWeapon.WasCanceledThisFrame
                 || MonoSingleton<InputManager>.Instance.InputSource.LastWeapon.WasCanceledThisFrame
                 || MonoSingleton<InputManager>.Instance.InputSource.PreviousVariation.WasCanceledThisFrame)
            {
                if (__instance.selectedSegment != -1)
                {
                    int target = __instance.segments[__instance.selectedSegment].slotIndex + 1;
                    MonoSingleton<GunControl>.Instance.SwitchWeapon(target);
                }

                __instance.gameObject.SetActive(value: false);
            }
            else
            {
                if (__instance.segments == null || __instance.segments.Count == 0)
                    return false;

                __instance.direction = Vector2.ClampMagnitude(__instance.direction + InputVars.MoveVector, 1f);
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
                    if ((bool)MonoSingleton<RumbleManager>.Instance)
                        MonoSingleton<RumbleManager>.Instance.SetVibration(RumbleProperties.WeaponWheelTick);
                }
            }
            return false;
        }
    }
}