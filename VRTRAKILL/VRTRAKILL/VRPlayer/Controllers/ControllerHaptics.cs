using HarmonyLib;
using UnityEngine;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    [HarmonyPatch(typeof(RumbleManager))] internal class ControllerHaptics
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(RumbleManager.Update))] static void Update()
        {

        }
        [HarmonyPostfix][HarmonyPatch(nameof(RumbleManager.OnDisable))] static void OnDisable()
        {

        }

        public SteamVR_Input_Sources ResolveController(string Key)
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
                    return SteamVR_Input_Sources.LeftHand;

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
                    return SteamVR_Input_Sources.RightHand;

                default:
                    Debug.LogError("No intensity found for key: " + Key);
                    return SteamVR_Input_Sources.Any;
            }
        }
    }
}
