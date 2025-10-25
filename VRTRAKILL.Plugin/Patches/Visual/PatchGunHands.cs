using HarmonyLib;
using VRTRAKILL.Systems;
using UnityEngine;

namespace VRTRAKILL.Patches.Visual;

[HarmonyPatch] internal class PatchGunHands
{
    [HarmonyPatch(typeof(Shotgun))] internal class TransformShotgun
    {
        static Vector3 Position = new(-.5f, -.95f, -.45f),
                       Rotation = new(0, 180, 0),
                       Scale    = new(1500, 1500, 1500);

        [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))]
        private static void Start(Shotgun __instance)
        {
            Transform Hand = Object.Instantiate(Assets.HandPose_Shotgun.transform);
            // Shotgun ******(Clone)/ShogunNewAnims/GunArmature/MainBone
            Hand.SetParent(__instance.transform.GetChild(2).GetChild(0).GetChild(0), false);
            Hand.localPosition = Vector3.zero;
            Hand.localEulerAngles = new Vector3(0, 0, 270);
            Hand.localScale = new Vector3(.1f, .1f, .1f);

            Hand.GetChild(1).GetChild(0).localPosition = Position;
            Hand.GetChild(1).GetChild(0).localEulerAngles = Rotation;
            Hand.GetChild(1).GetChild(0).localScale = Scale;
        }
    }

    [HarmonyPatch(typeof(Nailgun))] internal class NailgunH
    {
        static Vector3 Position    = new(-.0008f, -.0053f, .0003f),
                       Rotation    = new(0, 180, 0),
                       Scale       = new(.035f, .035f, .035f),
                       AltPosition = new(.001f, -.006f, .002f),
                       AltRotation = new(0, 0, 0),
                       AltScale    = new(3.5f, 3.5f, 3.5f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Nailgun.Start))]
        private static void Start(Nailgun __instance)
        {
            // Nailgun ******(Clone)/Nailgun New New/Armature/Main
            // Sawblade Launcher ******(Clone)/Sawblade Launcher/Armature/Base
            if (__instance.altVersion)
            {
                Transform Hand = Object.Instantiate(Assets.HandPose_Sawblade.transform);
                Hand.SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);
                Hand.localPosition = Vector3.zero;

                Hand.GetChild(1).GetChild(0).localPosition = AltPosition;
                Hand.GetChild(1).GetChild(0).localEulerAngles = AltRotation;
                Hand.GetChild(1).GetChild(0).localScale = AltScale;
            }
            else
            {
                Transform Hand = Object.Instantiate(Assets.HandPose_Nailgun.transform);
                Hand.SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);
                Hand.localPosition = Vector3.zero;

                Hand.localPosition = Position;
                Hand.localEulerAngles = Rotation;
                Hand.GetChild(1).GetChild(0).localScale = Scale;
            }
        }
    }

    [HarmonyPatch(typeof(Railcannon))] internal class RailgunH
    {
        static Vector3 Position = new(-.1f, -.325f, -.025f),
                       Rotation = new(30, 180, 0),
                       Scale    = new(350, 350, 350);

        [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))]
        private static void Start(Railcannon __instance)
        {
            Transform Hand = Object.Instantiate(Assets.HandPose_Railgun.transform);
            // Railcannon ******(Clone)/Railgun/Armature/Base
            Hand.SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);
            Hand.localPosition = Vector3.zero;

            Hand.GetChild(1).GetChild(0).localPosition = Position;
            Hand.GetChild(1).GetChild(0).localEulerAngles = Rotation;
            Hand.GetChild(1).GetChild(0).localScale = Scale;
        }
    }
}
