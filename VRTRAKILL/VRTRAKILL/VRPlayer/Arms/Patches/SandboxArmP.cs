using HarmonyLib;
using UnityEngine;
using Sandbox.Arm;
using UnityEngine.InputSystem.HID;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch(typeof(SandboxArm))] internal class SandboxArmP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(SandboxArm.Update))] static bool Update(SandboxArm __instance)
        {
            if (Time.timeScale == 0f)
            {
                return false;
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame
                && (__instance.currentMode == null || __instance.currentMode.CanOpenMenu))
            {
                __instance.menu.gameObject.SetActive(value: true);
                MonoSingleton<OptionsManager>.Instance.Freeze();
                return false;
            }

            if (!__instance.menu.gameObject.activeSelf)
            {
                if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo()
                    && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)
                    __instance.currentMode?.OnPrimaryDown();

                if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasCanceledThisFrame)
                    __instance.currentMode?.OnPrimaryUp();

                if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo()
                    && MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame)
                    __instance.currentMode?.OnSecondaryDown();

                if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasCanceledThisFrame)
                    __instance.currentMode?.OnSecondaryUp();
            }

            if (__instance.currentMode != null && __instance.currentMode.Raycast)
            {
                __instance.hitSomething =
                    Physics.Raycast(Vars.RightController.transform.position,
                                    Vars.RightController.transform.forward,
                                    out __instance.hit,
                                    75f,
                                    __instance.raycastLayers);
            }

            __instance.currentMode?.Update();
            return false;
        }
    }
}
