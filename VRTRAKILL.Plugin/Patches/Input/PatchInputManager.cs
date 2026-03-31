using HarmonyLib;
using VRTRAKILL.Systems.Input;

namespace VRTRAKILL.Patches.Input;

[HarmonyPatch(typeof(InputManager))] internal static class PatchInputManager
{
    [HarmonyPostfix, HarmonyPatch(nameof(InputManager.Awake))]
    private static void Awake()
    {
        SteamVRPlayerInput.Initialize();
    }
}