using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems.UI;

namespace VRTRAKILL.Patches.UI;

[HarmonyPatch(typeof(HUDPos))] internal class PatchHUDPos
{
    [HarmonyPrefix] [HarmonyPatch(nameof(HUDPos.Start))]
    static void Start(HUDPos __instance)
    {
        VRUIConverter.ConvertCanvas(__instance.GetComponent<Canvas>(), force: true, addComponent: false);
        __instance.gameObject.AddComponent<VRHideCanvasOnMenuActive>();

        __instance.transform.localScale = new Vector3(.5f, .35f, .5f);
        switch (__instance.gameObject.name)
        {
            // handpicked values for good visuals
            case "GunCanvas":
                StandardHUDToggle SHUDW = __instance.transform.parent.gameObject.AddComponent<StandardHUDToggle>();
                SHUDW.StandardHUD = __instance.gameObject;

                __instance.defaultPos = new Vector3(-640, -420, 120);
                __instance.defaultRot = new Vector3(0, -20, 0);

                __instance.reversePos = new Vector3(640, -420, 120);
                __instance.reverseRot = new Vector3(0, 20, 0);
                break;

            case "StyleCanvas":
                __instance.defaultPos = new Vector3(360, 60, -240);
                __instance.defaultRot = new Vector3(356, 45, 357);

                __instance.reversePos = new Vector3(-360, 60, -240);
                __instance.reverseRot = new Vector3(0, -45, 0);
                break;
        }
    }
}