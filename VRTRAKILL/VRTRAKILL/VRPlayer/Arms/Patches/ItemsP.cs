using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal class ItemsP
    {
        static ItemIdentifier ItemID { get; set; }
        static int ItemLM { get; set; }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Punch), nameof(Punch.Update))]
        [HarmonyPatch(typeof(Punch), nameof(Punch.PunchEnd))]
        static void ReLayerHeldItems(Punch __instance)
        {
            if (__instance.heldItem != null)
            {
                ItemID = __instance.heldItem; ItemLM = ItemID.gameObject.layer;
                ItemID.gameObject.layer = (int)Vars.Layers.Default;
            }
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(Punch), nameof(Punch.PunchStart))] static void LayerBackHeldItems(Punch __instance)
        {
            if (ItemID != null && __instance.heldItem == null)
            {
                ItemID.gameObject.layer = ItemLM;
                ItemID = null;
            }
        }
    }
}
