using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using VRTRAKILL.Data;

namespace VRTRAKILL.Systems;

internal static class CCTranspilerHelper
{
    public static MethodInfo GunRetarget
        => AccessTools.PropertyGetter(typeof(GlobalVars), nameof(GlobalVars.DominantHand));

    public static MethodInfo ArmRetarget
        => AccessTools.PropertyGetter(typeof(GlobalVars), nameof(GlobalVars.NonDominantHand));

    public static IEnumerable<MethodBase> GunTargetMethods()
    {
        yield return AccessTools.Method(typeof(Chainsaw), nameof(Chainsaw.Update));

        yield return AccessTools.Method(typeof(Nailgun), nameof(Nailgun.Shoot));
        // Disable camerashake?
        yield return AccessTools.Method(typeof(Nailgun), nameof(Nailgun.ShootMagnet));
        yield return AccessTools.Method(typeof(Nailgun), nameof(Nailgun.SuperSaw));
        yield return AccessTools.Method(typeof(Nailgun), nameof(Nailgun.ShootZapper));

        yield return AccessTools.Method(typeof(Railcannon), nameof(Railcannon.Shoot));

        yield return AccessTools.Method(typeof(RocketLauncher), nameof(RocketLauncher.Shoot));
        yield return AccessTools.Method(typeof(RocketLauncher), nameof(RocketLauncher.ShootCannonball));
        // Note that for napalm some used instance, is this right?
        yield return AccessTools.Method(typeof(RocketLauncher), nameof(RocketLauncher.ShootNapalm));

        yield return AccessTools.Method(typeof(Shotgun), nameof(Shotgun.Shoot));
        yield return AccessTools.Method(typeof(Shotgun), nameof(Shotgun.Update));
        yield return AccessTools.Method(typeof(Shotgun), nameof(Shotgun.ShootSinks));
        // Weird stuff at start
        yield return AccessTools.Method(typeof(Shotgun), nameof(Shotgun.ShootSaw));

        // chainsaw2 should it be from position?
        yield return AccessTools.Method(typeof(ShotgunHammer), nameof(ShotgunHammer.ImpactRoutine));
        // Same for this one?
        yield return AccessTools.Method(typeof(ShotgunHammer), nameof(ShotgunHammer.HitNade));
        // AND THIS?
        yield return AccessTools.Method(typeof(ShotgunHammer), nameof(ShotgunHammer.ImpactEffects));
        // Etc.
        yield return AccessTools.Method(typeof(ShotgunHammer), nameof(ShotgunHammer.ThrowNade));
        yield return AccessTools.Method(typeof(ShotgunHammer), nameof(ShotgunHammer.ShootSaw));
        // But not this one
        yield return AccessTools.Method(typeof(ShotgunHammer), nameof(ShotgunHammer.Update));

        yield return AccessTools.Method(typeof(Vacuum), nameof(Vacuum.SuckObjects));
        yield return AccessTools.Method(typeof(Vacuum), nameof(Vacuum.UpdateStuckObject));

        yield return AccessTools.Method(typeof(Washer), nameof(Washer.Update));

        yield return AccessTools.Method(typeof(Revolver), nameof(Revolver.Shoot));
    }

    public static IEnumerable<MethodBase> ArmsTargetMethods()
    {
        // TODO fill
        yield return AccessTools.Method(typeof(Punch), nameof(Punch.Update));
    }

    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodInfo retarget)
    {
        // this.cc, MonoSingleton<CameraController>.Instance, Camera.main, this.cam
        var match = new CodeMatch(i =>
            i.opcode == OpCodes.Ldfld && i.operand is FieldInfo f && f.FieldType == typeof(CameraController)
         || i.opcode == OpCodes.Call && i.operand is MethodInfo f2 && f2 == AccessTools.PropertyGetter(typeof(CameraController), nameof(CameraController.Instance))
         || i.opcode == OpCodes.Call && i.operand is MethodInfo f3 && f3 == AccessTools.PropertyGetter(typeof(Camera), nameof(Camera.main))
         || i.opcode == OpCodes.Ldfld && i.operand is FieldInfo f4 && (f4.Name.ToLower() == "cam" || f4.Name.ToLower() == "camobj"));

        // .position
        var sequence = new CodeMatcher(instructions)
            .MatchForward(false, match,
            new(i => i.opcode == OpCodes.Callvirt && i.operand is MethodInfo m && m.Name == "get_transform"))
            .Repeat(matcher =>
            {
                //If we load a field we need to get rid of the thing currently on the stack (this)
                if (matcher.Instruction.opcode == OpCodes.Ldfld)
                    matcher.InsertAndAdvance(new CodeInstruction(OpCodes.Pop));
                
                var @new = new CodeInstruction(OpCodes.Call, retarget);
                @new.labels.AddRange(matcher.Instruction.labels);
                matcher.SetInstructionAndAdvance(@new);

                var transform = AccessTools.Method(typeof(GameObject), "get_transform");
                matcher.SetInstructionAndAdvance(new CodeInstruction(OpCodes.Callvirt, transform));
            })
            .InstructionEnumeration();
        return sequence;
    }
}