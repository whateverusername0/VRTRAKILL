using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.Patches
{
    [HarmonyPatch] internal class HudP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(HUDPos), nameof(HUDPos.Start))] static void ReparentHUD(HUDPos __instance)
        {
            __instance.transform.parent = HUDOptions.Instance.transform;

            UIConverter.ConvertCanvas(__instance.GetComponent<Canvas>(), Force: true, DontAddComponent: true);
            __instance.gameObject.AddComponent<HideWhenMenuActive>();
            
            switch (__instance.gameObject.name)
            {
                case "GunCanvas":
                    StandardHUDWorker SHUDW = __instance.transform.parent.gameObject.AddComponent<StandardHUDWorker>();
                    SHUDW.StandardHUD = __instance.gameObject;

                    __instance.defaultPos = new Vector3(-520, -240, 460); __instance.transform.localPosition = new Vector3(-520, -240, 460);
                    __instance.defaultRot = new Vector3(355, 315, 1.5f); __instance.transform.localEulerAngles = new Vector3(355, 315, 1.5f);
                    // reversepos
                    // reverserot
                    
                    break;
                case "StyleCanvas":
                    __instance.defaultPos = new Vector3(324, 48, -286); __instance.transform.localPosition = new Vector3(324, 48, -286);
                    __instance.defaultRot = new Vector3(0, 56, 0); __instance.transform.localEulerAngles = new Vector3(0, 56, 0);
                    // reversepos
                    // reverserot
                    break;
            }
            __instance.transform.localScale = new Vector3(.5f, .5f, .5f);
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(FinalRank), nameof(FinalRank.Start))] static void ReparentFR(FinalRank __instance)
        {
            __instance.transform.parent.transform.parent = HUDOptions.Instance.transform;

            UIConverter.ConvertCanvas(__instance.transform.parent.GetComponent<Canvas>(), Force: true);

            __instance.transform.localScale = new Vector3(16, 16, 16);
        }
    }
}
