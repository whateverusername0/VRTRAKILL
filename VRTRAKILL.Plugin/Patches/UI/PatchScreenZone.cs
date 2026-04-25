using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems;
using VRTRAKILL.Systems.UI;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Patches.UI;

[HarmonyPatch(typeof(ScreenZone))] internal class PatchScreenZone
{
    [HarmonyPostfix] [HarmonyPatch(nameof(ScreenZone.OnTriggerEnter))]
    static void OnTriggerEnter()
    {
        // convert again
        foreach (Canvas C in Resources.FindObjectsOfTypeAll(typeof(Canvas)))
            if (!C.gameObject.HasComponent<VRCanvas>())
                VRCanvasHelper.RecursiveConvertCanvas();
    }
}
