using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems;
using VRTRAKILL.Systems.Arms;
using VRTRAKILL.Systems.VRAvatar.Armature;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(WeaponIdentifier))] internal static class PatchWeaponIdentifier
{
    [HarmonyPostfix] [HarmonyPatch(nameof(WeaponIdentifier.Start))]
    static void Transform(WeaponIdentifier __instance)
    {
        var washer = __instance.GetComponentInChildren<Washer>();
        if (washer)
        {
            WeaponTransform.ApplyTransform(__instance.GetComponent<WeaponPos>(), new(0.065f, -0.075f, -0.175f), new(10, 90, 20), new(-.0025f, .0025f, .0025f));

            var WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
            var A = Arm.FeedbackerPreset(__instance.transform);
            WAC.Arm = A; WAC.OffsetPos = Vector3.zero;

            washer.defaultSprayPos = new(.35f, -4.25f, -.675f);
            washer.defaultSprayRot = Quaternion.Euler(90, 0, 0);

            var sprayStart = __instance.GetComponentInChildren<CorrectCameraView>();
            if (sprayStart)
            {
                sprayStart.transform.localPosition = new(.35f, -4.25f, -.675f);
                sprayStart.transform.localEulerAngles = new(90, 0, 0);
                sprayStart.enabled = false;
            }
            return;
        }

        var vacuum = __instance.GetComponentInChildren<Vacuum>();
        if (vacuum)
        {
            WeaponTransform.ApplyTransform(__instance.GetComponent<WeaponPos>(), new(0, -.35f, 2.15f), new(0, 90, 20), new(.125f, .125f, .125f));

            var WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
            var A = Arm.FeedbackerPreset(__instance.transform);
            WAC.Arm = A; WAC.OffsetPos = Vector3.zero;
            return;
        }
    }
}