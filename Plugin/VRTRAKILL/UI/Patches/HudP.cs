using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] internal class HudP
    {
        [HarmonyPostfix] [HarmonyPatch(typeof(HudController), nameof(HudController.Start))] static void ReparentHUD(HudController __instance)
        {
            if (__instance.altHud) return;
            else
            {
                __instance.transform.parent = CanvasController.Instance.transform;
                __instance.transform.localPosition = Vector3.zero;
                __instance.transform.localEulerAngles = Vector3.zero;
            }
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(HUDPos), nameof(HUDPos.Start))] static void RetransformElements(HUDPos __instance)
        {
            UIConverter.ConvertCanvas(__instance.GetComponent<Canvas>(), Force: true, DontAddComponent: true);
            __instance.gameObject.AddComponent<HideWhenMenuActive>();
            
            switch (__instance.gameObject.name)
            {
                case "GunCanvas":
                    StandardHUDWorker SHUDW = __instance.transform.parent.gameObject.AddComponent<StandardHUDWorker>();
                    SHUDW.StandardHUD = __instance.gameObject;

                    __instance.transform.localScale = new Vector3(.25f, .25f, .25f);
                    __instance.defaultPos = new Vector3(-360, -220, -240);
                    __instance.defaultRot = new Vector3(0, -45, 0);

                    __instance.reversePos = new Vector3(360, -220, -240);
                    __instance.reverseRot = new Vector3(0, 45, 0);
                    break;

                case "StyleCanvas":
                    __instance.defaultPos = new Vector3(320, 60, -240);
                    __instance.defaultRot = new Vector3(356, 65, 357);

                    __instance.reversePos = new Vector3(-320, 60, -240);
                    __instance.reverseRot = new Vector3(0, -65, 0);
                    break;
            }
            __instance.transform.localScale = new Vector3(.5f, .5f, .5f);
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(FinalRank), nameof(FinalRank.Start))] static void ConvertFinalRank(FinalRank __instance)
        {
            UIConverter.ConvertCanvas(__instance.transform.parent.GetComponent<Canvas>(), Force: true);
            __instance.transform.localPosition = Vector3.zero;
            __instance.transform.localScale = new Vector3(16, 16, 16);
        }
    }
}
