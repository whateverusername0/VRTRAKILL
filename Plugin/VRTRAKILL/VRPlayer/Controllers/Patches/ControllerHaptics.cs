using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers.Patches
{
    [HarmonyPatch(typeof(RumbleManager))] internal sealed class ControllerHaptics
    {
        static readonly SteamVR_Action_Vibration HapticAction = SteamVR_Actions._default.Haptic;

        // I can't believe it worked first try without any corrections.
        [HarmonyPrefix] [HarmonyPatch(nameof(RumbleManager.Update))] static bool Update(RumbleManager __instance)
        {
            __instance.discardedKeys.Clear();
            foreach (KeyValuePair<RumbleKey, PendingVibration> pendingVibration in __instance.pendingVibrations)
            {
                if (pendingVibration.Value.isTracking && (pendingVibration.Value.trackedObject == null || !pendingVibration.Value.trackedObject.activeInHierarchy))
                    __instance.discardedKeys.Add(pendingVibration.Key);
                else if (pendingVibration.Value.IsFinished)
                    __instance.discardedKeys.Add(pendingVibration.Key);
            }

            foreach (RumbleKey discardedKey in __instance.discardedKeys)
                __instance.pendingVibrations.Remove(discardedKey);

            float num = 0f;
            SteamVR_Input_Sources source = 0;
            foreach (KeyValuePair<RumbleKey, PendingVibration> pendingVibration2 in __instance.pendingVibrations)
                if (pendingVibration2.Value.Intensity > num)
                {
                    num = pendingVibration2.Value.Intensity;
                    source = ResolveController(pendingVibration2.Key.name);
                }

            num *= MonoSingleton<PrefsManager>.Instance.GetFloat("totalRumbleIntensity");
            if ((bool)MonoSingleton<OptionsManager>.Instance && MonoSingleton<OptionsManager>.Instance.paused)
                num = 0f;

            Vibrate(1, num, num, source);

            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(nameof(RumbleManager.OnDisable))]
        [HarmonyPatch(nameof(RumbleManager.StopVibration))]
        [HarmonyPatch(nameof(RumbleManager.StopAllVibrations))]
        static bool DisableRumble()
        {
            Vibrate(1, 0, 0, 0);
            return false;
        }

        // Number 7:
        public static void Vibrate(float Duration, float Frequency, float Amplitude, SteamVR_Input_Sources Source)
        { HapticAction.Execute(0, Duration, Frequency, Amplitude, Source); }

        public static SteamVR_Input_Sources ResolveController(string Key)
        {
            switch (Key)
            {
                case "rumble.slide":
                case "rumble.dash":
                case "rumble.fall_impact":
                case "rumble.jump":
                case "rumble.fall_impact_heave":
                case "rumble.weapon_wheel_tick":
                    return SteamVR_Input_Sources.Any;

                case "rumble.punch":
                case "rumble.parry_flash":
                case "rumble.coin_toss":
                case "rumble.whiplash.throw":
                case "rumble.whiplash.pull":
                    return Vars.NDHC.GetComponent<SteamVR_Behaviour_Pose>().inputSource;

                case "rumble.gun.fire":
                case "rumble.gun.fire_strong":
                case "rumble.gun.fire_projectiles":
                case "rumble.gun.railcannon_idle":
                case "rumble.gun.nailgun_fire":
                case "rumble.gun.super_saw":
                case "rumble.gun.shotgun_charge":
                case "rumble.gun.sawblade":
                case "rumble.gun.revolver_charge":
                case "rumble.magnet_released":
                    return Vars.DHC.GetComponent<SteamVR_Behaviour_Pose>().inputSource;

                default:
                    Debug.LogError("No intensity found for key: " + Key);
                    return SteamVR_Input_Sources.Any;
            }
        }
    }
}
