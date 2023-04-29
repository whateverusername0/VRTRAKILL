using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    // Dual wielding has this funny bug where it makes third, fourth guns far far away and it looks cool but no
    [HarmonyPatch(typeof(DualWieldPickup))] internal class DoubleWieldP
    {
        [HarmonyPrefix] [HarmonyPatch(nameof(DualWieldPickup.PickedUp))] static bool FixTransform(DualWieldPickup __instance)
        {
            if (!MonoSingleton<GunControl>.Instance) return false;

            Object.Instantiate(__instance.pickUpEffect, __instance.transform.position, Quaternion.identity);
            MonoSingleton<CameraController>.Instance.CameraShake(0.35f);
            __instance.gameObject.SetActive(false);
            if (MonoSingleton<PlayerTracker>.Instance.playerType == PlayerType.Platformer)
            {
                MonoSingleton<PlatformerMovement>.Instance.AddExtraHit(3);
                return false;
            }

            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(MonoSingleton<GunControl>.Instance.transform, worldPositionStays: true);
            gameObject.transform.localRotation = Quaternion.identity;
            DualWield[] componentsInChildren = MonoSingleton<GunControl>.Instance.GetComponentsInChildren<DualWield>();

            gameObject.transform.localScale = Vector3.one;

            // no more.
            if (componentsInChildren == null || componentsInChildren.Length == 0)
                gameObject.transform.localPosition = Vector3.zero;
            else if (componentsInChildren.Length % 2 == 0)
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
}
