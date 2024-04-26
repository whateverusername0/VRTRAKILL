using HarmonyLib;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Guns.Patches
{
    [HarmonyPatch] internal sealed class AutoAimP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(RotateToFaceFrustumTarget), nameof(RotateToFaceFrustumTarget.Update))] static bool Update(RotateToFaceFrustumTarget __instance)
        {
            __instance.targeter.camera = VRGunsController.Instance.TargeterCamera;

            Quaternion to = Vars.DominantHand.transform.rotation;
            if ((bool)__instance.targeter && __instance.targeter.isActiveAndEnabled && CameraFrustumTargeter.IsEnabled && (bool)__instance.targeter.CurrentTarget)
                to = Quaternion.LookRotation(__instance.targeter.CurrentTarget.bounds.center - __instance.transform.position);

            __instance.transform.rotation = to;
            return false;
        }
    }
}
