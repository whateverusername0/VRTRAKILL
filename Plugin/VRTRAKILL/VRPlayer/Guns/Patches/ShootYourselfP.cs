using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] internal class ShootYourselfP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(RevolverBeam), nameof(RevolverBeam.HitSomething))] static void RBeam(RaycastHit hit)
        {
            if (hit.transform == null) return;
            if (hit.transform.gameObject.tag == "Player")
            {
                NewMovement.Instance.GetHurt(100, false);
                StyleHUD.Instance.RemovePoints((int)StyleHUD.Instance.currentMeter);
            }
        }
    }
}
