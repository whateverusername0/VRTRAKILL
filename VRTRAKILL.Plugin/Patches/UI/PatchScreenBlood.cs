using HarmonyLib;

namespace VRTRAKILL.Patches.UI;

[HarmonyPatch(typeof(ScreenBlood))] internal class PatchScreenBlood
{
    [HarmonyPostfix] [HarmonyPatch(nameof(ScreenBlood.Start))]
    static void Start(ScreenBlood __instance)
    {
        // makes blood splatters bigger.
        __instance.transform.localScale *= 10;
    }
}