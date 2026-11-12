using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace VRTRAKILL.Plugin.Patches.Cameras;

[HarmonyPatch]
internal static class FrustumTargeterTranspiler
{
    // Lists the weapon methods that get the frustum targeter from Camera.main.
    private static IEnumerable<MethodBase> TargetMethods()
    {
        yield return AccessTools.Method(typeof(Railcannon), nameof(Railcannon.GetStuff));
        yield return AccessTools.Method(typeof(ShotgunHammer), nameof(ShotgunHammer.Awake));
        yield return AccessTools.Method(typeof(SecondaryRevolver), nameof(SecondaryRevolver.Start));
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        // Camera.main.GetComponent<CameraFrustumTargeter>() -> CameraFrustumTargeter.Instance
        return new CodeMatcher(instructions)
            .MatchForward(false,
            new(i => i.opcode == OpCodes.Call && i.operand is MethodInfo f2 && f2 == AccessTools.PropertyGetter(typeof(Camera), nameof(Camera.main))),
            new(i => i.opcode == OpCodes.Callvirt && i.operand.ToString().Contains(nameof(CameraFrustumTargeter))))
            .SetInstructionAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.PropertyGetter(typeof(CameraFrustumTargeter), nameof(CameraFrustumTargeter.Instance))))
            .InstructionEnumeration();
    }
}