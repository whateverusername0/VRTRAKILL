using HarmonyLib;

namespace VRTRAKILL.Patches.UI;

[HarmonyPatch(typeof(FlashImage))] internal static class PatchFlashImage
{
    // fuck you
    [HarmonyPostfix] [HarmonyPatch(nameof(FlashImage.Flash))]
    static void Flash(FlashImage __instance)
    {
        if (__instance.gameObject.name.Contains("White") || __instance.gameObject.name.Contains("Black"))
            __instance.transform.localScale *= 20;
    }
}
