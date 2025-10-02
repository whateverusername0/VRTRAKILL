using HarmonyLib;
using Plugin.Data;
using Plugin.Systems.Input;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ULTRAKILL.Cheats;
using UnityEngine;

namespace Plugin.Patches.ULTRAKILL.Movement;

[HarmonyPatch(typeof(NewMovement))] internal class PatchNewMovement
{
    [HarmonyPatch(nameof(NewMovement.Start))]
    private static void Start(NewMovement __instance)
    {
        __instance.walkSpeed *= Vars.Config.MovementMultiplier;
        __instance.jumpPower *= Vars.Config.MovementMultiplier;
        __instance.wallJumpPower *= Vars.Config.MovementMultiplier;
    }
}