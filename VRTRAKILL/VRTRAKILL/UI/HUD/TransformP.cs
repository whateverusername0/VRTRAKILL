using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.UI.HUD
{
    [HarmonyPatch] internal class TransformP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(HUDPos), nameof(HUDPos.Start))] static void ReparentHUD(HUDPos __instance)
        {
            __instance.transform.parent = HUDOptions.Instance.transform;

            if (!Vars.Config.VRSettings.VRUI.EnableStandardHUD) __instance.GetComponent<Canvas>().enabled = false;

            UIConverter.ConvertCanvas(__instance.GetComponent<Canvas>(), Force: true, DontAddComponent: true);

            switch (__instance.gameObject.name)
            {
                case "GunCanvas":
                    __instance.defaultPos = new Vector3(-520, -240, 460); __instance.transform.localPosition = new Vector3(-520, -240, 460);
                    __instance.defaultRot = new Vector3(355, 315, 1.5f); __instance.transform.localEulerAngles = new Vector3(355, 315, 1.5f);
                    __instance.transform.localScale = new Vector3(.5f, .5f, .5f);
                    break;
                case "StyleCanvas":
                    __instance.defaultPos = new Vector3(324, 48, -286); __instance.transform.localPosition = new Vector3(324, 48, -286);
                    __instance.defaultRot = new Vector3(0, 56, 0); __instance.transform.localEulerAngles = new Vector3(0, 56, 0);
                    __instance.transform.localScale = new Vector3(.5f, .5f, .5f);
                    break;
            }
        }
        [HarmonyPostfix] [HarmonyPatch(typeof(FinalRank), nameof(FinalRank.Start))] static void ReparentFR(FinalRank __instance)
        {
            __instance.transform.parent.transform.parent = HUDOptions.Instance.transform;

            UIConverter.ConvertCanvas(__instance.transform.parent.GetComponent<Canvas>(), Force: true);

            __instance.transform.localScale = new Vector3(16, 16, 16);
        }
    }
}
