using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.VRCameraController;

[HarmonyPatch(typeof(CameraController))] internal static class TranspilerArmCameraController
{
    private static IEnumerable<MethodBase> TargetMethods()
        => CCTranspilerHelper.ArmsTargetMethods();

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        => CCTranspilerHelper.Transpiler(instructions, CCTranspilerHelper.ArmRetarget);
}