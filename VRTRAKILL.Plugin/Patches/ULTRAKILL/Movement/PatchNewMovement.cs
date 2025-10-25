using HarmonyLib;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.Input;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace VRTRAKILL.Patches.ULTRAKILL.Movement;

[HarmonyPatch(typeof(NewMovement))] internal class PatchNewMovement
{
    [HarmonyPatch(nameof(NewMovement.Start))]
    private static void Start(NewMovement __instance)
    {
        __instance.walkSpeed *= Vars.Config.UpdateMultiplier;
        __instance.jumpPower *= Vars.Config.UpdateMultiplier;
        __instance.wallJumpPower *= Vars.Config.UpdateMultiplier;
    }
}