using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.VRCameraController;

[HarmonyPatch(typeof(CameraController))] internal static class TranspilerGunCameraController
{
    private static IEnumerable<MethodBase> TargetMethods()
        => CCTranspilerHelper.GunTargetMethods();

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        => CCTranspilerHelper.Transpiler(instructions, CCTranspilerHelper.GunRetarget);
}