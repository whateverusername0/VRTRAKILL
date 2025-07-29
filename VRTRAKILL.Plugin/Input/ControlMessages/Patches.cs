using HarmonyLib;

namespace VRBasePlugin.ULTRAKILL.Input.ControlMessages
{
    [HarmonyPatch] internal static class Patches
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(HudMessage), nameof(HudMessage.PlayMessage))] static void PlayMessage(HudMessage __instance, bool hasToBeEnabled = false)
        {
            if (ConvertTextToVR(__instance.message, __instance.message2) == "Error"
            && (!string.IsNullOrEmpty(__instance.message) || !string.IsNullOrEmpty(__instance.message2))) return;
            else __instance.ChangeMessage(ConvertTextToVR(__instance.message, __instance.message2));
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(TextBinds), nameof(TextBinds.OnEnable))] static void TBOnEnable(TextBinds __instance)
        {
            if (ConvertTextToVR(__instance.text1, __instance.text2) == "Error"
            && (!string.IsNullOrEmpty(__instance.text1) || !string.IsNullOrEmpty(__instance.text2))) return;
            else __instance.text.text = ConvertTextToVR(__instance.text1, __instance.text2);
        }
        public static string ConvertTextToVR(string Message, string Message2)
        {
            string FullMessage = Message + Message2;
            foreach (string Key in MessageContainer.HintsToTexts.Keys)
            {
                if (FullMessage.Contains(Key))
                    if (MessageContainer.HintsToTexts.TryGetValue(Key, out string Value) && Value != null)
                        return Value;
            }
            return "Error";
        }
    }
}
