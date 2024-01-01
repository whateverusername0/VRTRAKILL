﻿using HarmonyLib;
using VRBasePlugin.ULTRAKILL.VRPlayer.VRAvatar.Armature;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal sealed class ArmsP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(Punch), nameof(Punch.Start))] static void ConvertArms(Punch __instance)
        {
            Arm A = null;
            switch (__instance.type)
            {
                case FistType.Standard: A = Arm.FeedbackerPreset(__instance.transform); break;
                case FistType.Heavy: A = Arm.KnuckleblasterPreset(__instance.transform); break;
                case FistType.Spear: default: break;
            }
            ArmTransformer AT = __instance.gameObject.AddComponent<ArmTransformer>();
            ArmController.ACBase AC = __instance.gameObject.AddComponent<ArmController.DefaultArmCon>();
            AT.Arm = A; AC.Arm = A;

            // stop
            foreach (SkinnedMeshRenderer SMR in __instance.GetComponentsInChildren<SkinnedMeshRenderer>())
                SMR.updateWhenOffscreen = true;
        }
    }
}
