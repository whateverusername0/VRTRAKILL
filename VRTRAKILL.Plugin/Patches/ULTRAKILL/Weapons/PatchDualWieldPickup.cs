using HarmonyLib;
using UnityEngine;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons;

// Dual wielding has this bug where it makes third, fourth guns far far away.
// This patch fixes it.
[HarmonyPatch(typeof(DualWieldPickup))] internal class PatchDualWieldPickup
{
    [HarmonyPrefix] [HarmonyPatch(nameof(DualWieldPickup.PickedUp))]
    private static bool FixTransform(DualWieldPickup __instance)
    {
        if (!GunControl.Instance) return false;

        Object.Instantiate(__instance.pickUpEffect, __instance.transform.position, Quaternion.identity);
        CameraController.Instance.CameraShake(0.35f);
        __instance.gameObject.SetActive(false);
        if (PlayerTracker.Instance.playerType == PlayerType.Platformer)
        {
            PlatformerMovement.Instance.AddExtraHit(3);
            return false;
        }

        var gameObject = new GameObject();
        gameObject.transform.SetParent(GunControl.Instance.transform, worldPositionStays: true);
        gameObject.transform.localRotation = Quaternion.identity;
        DualWield[] componentsInChildren = GunControl.Instance.GetComponentsInChildren<DualWield>();

        gameObject.transform.localScale = Vector3.one;

        /* if (componentsInChildren == null || componentsInChildren.Length == 0)
            gameObject.transform.localPosition = Vector3.zero;
        else */ if (componentsInChildren.Length % 2 == 0)
            gameObject.transform.localPosition = new Vector3((float)(componentsInChildren.Length / 2) * -.15f, 0f, 0f);
        else
            gameObject.transform.localPosition = new Vector3((float)((componentsInChildren.Length) / 2) * .15f, 0f, 0f);

        DualWield dualWield = gameObject.AddComponent<DualWield>();
        dualWield.delay = 0.05f;
        dualWield.juiceAmount = __instance.juiceAmount;

        if (componentsInChildren != null && componentsInChildren.Length != 0) dualWield.delay += (float)componentsInChildren.Length / 20f;

        return false;
    }
}
