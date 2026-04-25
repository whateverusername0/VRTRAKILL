using HarmonyLib;
using System.Collections.Generic;
using VRTRAKILL.Systems.Input;

namespace VRTRAKILL.Patches.Input;

[HarmonyPatch(typeof(InputActionState))] internal static class PatchInputActionState
{
    private static readonly Dictionary<string, object> _map = new()
    {
        { nameof(PlayerInput.Move), SteamVRPlayerInput.MoveVector },
        { nameof(PlayerInput.Look), SteamVRPlayerInput.LookVector },
        { nameof(PlayerInput.WheelLook), SteamVRPlayerInput.WheelLook },
    };

    // player.move.ReadValue translates to VRPlayerInput.Move.ReadValue, which is good.
    [HarmonyPrefix] [HarmonyPatch(nameof(InputActionState.ReadValue))]
    private static bool ReadValue<T>(InputActionState __instance, ref T __result) where T : struct
    {
        var name = __instance.Action.name;
        if (!_map.ContainsKey(name))
            return true;

        __result = (T)_map[name];
        return false;
    }
}