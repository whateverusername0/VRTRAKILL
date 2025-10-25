using HarmonyLib;
using VRTRAKILL.Data;
using System.Collections.Generic;

namespace VRTRAKILL.Patches.ULTRAKILL;

[HarmonyPatch] internal class PatchValuesGeneric
{
    /// <summary>
    ///     See <see cref="PlayerInput"/> or <see cref="InputActions"/> for reference.
    /// </summary>
    private static readonly Dictionary<string, object> _pairs = new()
    {
        { nameof(InputActions.MovementActions.Move), InputVars.MoveVector }
    };

    [HarmonyPrefix] [HarmonyPatch(typeof(InputActionState), nameof(InputActionState.ReadValue))]
    private static bool ReadValue<T>(InputActionState __instance, ref T __result) where T : struct
    {
        var name = __instance.Action.name;
        if (!_pairs.ContainsKey(name))
            return true;

        __result = (T)_pairs[name];
        return false;
    }
}