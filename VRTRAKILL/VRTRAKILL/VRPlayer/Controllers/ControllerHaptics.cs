using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    [HarmonyPatch(typeof(RumbleManager))] internal class ControllerHaptics
    {
        static SteamVR_Action_Vibration HapticAction = SteamVR_Actions._default.Haptic;

        [HarmonyPostfix] [HarmonyPatch(nameof(RumbleManager.Update))] static void Update(RumbleManager __instance)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, PendingVibration> keyValuePair in __instance.pendingVibrations)
            {
                if (keyValuePair.Value.isTracking && (keyValuePair.Value.trackedObject == null || !keyValuePair.Value.trackedObject.activeInHierarchy))
                    list.Add(keyValuePair.Key);
                else if (keyValuePair.Value.IsFinished)
                    list.Add(keyValuePair.Key);
            }
            foreach (string key in list)
            {
                __instance.pendingVibrations.Remove(key);
            }
            float Num = 0f;
            SteamVR_Input_Sources Source = 0;
            foreach (KeyValuePair<string, PendingVibration> keyValuePair2 in __instance.pendingVibrations)
            {
                if (keyValuePair2.Value.Intensity > Num)
                    Num = keyValuePair2.Value.Intensity;
                Source = ResolveController(keyValuePair2.Key);
            }
            Num *= MonoSingleton<PrefsManager>.Instance.GetFloat("totalRumbleIntensity", 0f);
            if (MonoSingleton<OptionsManager>.Instance && MonoSingleton<OptionsManager>.Instance.paused) Num = 0f;
            __instance.currentIntensity = Num;

            if (Vars.Config.VRInputSettings.Hands.EnableCH)
                Vibrate(1, Num, Num, Source);
        }
        [HarmonyPostfix][HarmonyPatch(nameof(RumbleManager.OnDisable))] static void OnDisable()
        {
            // Dunno if this is needed, not removing it doe
            Vibrate(1, 0, 0, 0);
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
                    if (Vars.Config.VRInputSettings.Hands.LeftHandMode) return SteamVR_Input_Sources.RightHand;
                    else return SteamVR_Input_Sources.LeftHand;

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
                    if (Vars.Config.VRInputSettings.Hands.LeftHandMode) return SteamVR_Input_Sources.LeftHand;
                    else return SteamVR_Input_Sources.RightHand;

                default:
                    Debug.LogError("No intensity found for key: " + Key);
                    return SteamVR_Input_Sources.Any;
            }
        }
    }
}
