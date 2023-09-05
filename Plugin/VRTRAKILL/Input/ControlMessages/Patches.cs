using HarmonyLib;
using System;
using UnityEngine;
using WindowsInput.Native;

namespace Plugin.VRTRAKILL.Input.ControlMessages
{
    [HarmonyPatch] internal static class Patches
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(HudMessage), nameof(HudMessage.PlayMessage))] static void PlayMessage(HudMessage __instance, bool hasToBeEnabled = false)
        {
            if (ResolveThing(__instance.message, __instance.message2) == "Error"
            && (!string.IsNullOrEmpty(__instance.message) || !string.IsNullOrEmpty(__instance.message2))) return;
            else __instance.ChangeMessage(ResolveThing(__instance.message, __instance.message2));
        }

        public static string ResolveThing(string Message, string Message2)
        {
            string FullMessage = Message + Message2;
            if (FullMessage.Contains("PUNCH")) return MessageContainer.T_Punch;
            else if (FullMessage.Contains("SLIDE")) return MessageContainer.T_Slide;

            switch (FullMessage)
            {
                case string A when A.Contains("PUNCH"): return MessageContainer.T_Punch;
                case string A when A.Contains("SLIDE"):  return MessageContainer.T_Slide;
                case string A when A.Contains("DASH"): return MessageContainer.T_JumpDash;
                case string A when A.Contains("SHOCKWAVE"): return MessageContainer.T_Slam;
                case string A when A.Contains("deals damage on direct hit."): return MessageContainer.T_Prelude_Slam;
                case string A when A.Contains("REVOLVER"): return MessageContainer.T_Revolver;
                case string A when A.Contains("SHOTGUN"): return MessageContainer.T_Shotgun;
                case string A when A.Contains("NAILGUN"): return MessageContainer.T_Nailgun;
                case string A when A.Contains("arms with"): return MessageContainer.T_Knuckleblaster;
                case string A when A.Contains("Only the"): return MessageContainer.T_Lust_ArmSwapReminder;
                case string A when A.Contains("to throw, release to pull"): return MessageContainer.T_Whiplash;
                default: return "Error";
            }
        }
    }
}
