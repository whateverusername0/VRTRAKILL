using HarmonyLib;
using VRTRAKILL.Data;
using UnityEngine;
using Valve.VR;
using System.Linq;

namespace VRTRAKILL.Patches.Controllers;

// TODO rework
//[HarmonyPatch(typeof(RumbleManager))] internal static class PatchRumbleManager
//{
//    static SteamVR_Input_Sources _nonDomHand => GlobalVars.ArmsController.GetComponent<SteamVR_Behaviour_Pose>().inputSource;
//    static SteamVR_Input_Sources _domHand => GlobalVars.GunsController.GetComponent<SteamVR_Behaviour_Pose>().inputSource;

//    [HarmonyPostfix] [HarmonyPatch(nameof(RumbleManager.Update))]
//    static void Update(RumbleManager __instance)
//    {
//        var any = __instance.pendingVibrations.Max((q) => q.Value.Intensity);
//        var nd = __instance.pendingVibrations.Where((q) => ResolveController(q.Key.name) == _nonDomHand).Max(q => q.Value.Intensity);
//        var d = __instance.pendingVibrations.Where((q) => ResolveController(q.Key.name) == _domHand).Max(q => q.Value.Intensity);

//        if (any > 0) Vibrate(1, any, any, SteamVR_Input_Sources.Any);
//        if (nd > 0) Vibrate(1, nd, nd, _nonDomHand);
//        if (d > 0) Vibrate(1, d, d, _domHand);
//    }

//    [HarmonyPrefix]
//    [HarmonyPatch(nameof(RumbleManager.OnDisable))]
//    [HarmonyPatch(nameof(RumbleManager.StopVibration))]
//    [HarmonyPatch(nameof(RumbleManager.StopAllVibrations))]
//    static bool DisableRumble()
//    {
//        Vibrate(1, 0, 0, 0);
//        return false;
//    }

//    // Number 7:
//    public static void Vibrate(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
//    { } //HapticAction.Execute(0, duration, frequency, amplitude, source);

//    public static SteamVR_Input_Sources ResolveController(string key)
//    {
//        switch (key)
//        {
//            case "rumble.parry_flash":
//            case "rumble.slide":
//            case "rumble.dash":
//            case "rumble.fall_impact":
//            case "rumble.jump":
//            case "rumble.fall_impact_heave":
//            case "rumble.weapon_wheel_tick":
//                return SteamVR_Input_Sources.Any;

//            case "rumble.punch":
//            case "rumble.coin_toss":
//            case "rumble.whiplash.throw":
//            case "rumble.whiplash.pull":
//                return _nonDomHand;

//            case "rumble.gun.fire":
//            case "rumble.gun.fire_strong":
//            case "rumble.gun.fire_projectiles":
//            case "rumble.gun.railcannon_idle":
//            case "rumble.gun.nailgun_fire":
//            case "rumble.gun.super_saw":
//            case "rumble.gun.shotgun_charge":
//            case "rumble.gun.sawblade":
//            case "rumble.gun.revolver_charge":
//            case "rumble.magnet_released":
//                return _domHand;

//            default:
//                Debug.LogError("No intensity found for key: " + key);
//                return SteamVR_Input_Sources.Any;
//        }
//    }
//}
