using HarmonyLib;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] internal sealed class AddHandsP
    {
        [HarmonyPatch(typeof(Shotgun))] static class ShotgunH
        {
            static Vector3 Position = new Vector3(-.5f, -.95f, -.45f),
                           Rotation = new Vector3(0, 180, 0),
                           Scale    = new Vector3(1500, 1500, 1500);

            [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))] static void AddHand(Shotgun __instance)
            {
                Transform Hand = Object.Instantiate(Assets.HandPose_Shotgun.transform);
                // Shotgun ******(Clone)/Shotgun_New/GunArmature/MainBone
                Hand.SetParent(__instance.transform.GetChild(2).GetChild(2).GetChild(0), false);
                Hand.localPosition = Vector3.zero;

                Hand.GetChild(1).GetChild(0).localPosition = Position;
                Hand.GetChild(1).GetChild(0).localEulerAngles = Rotation;
                Hand.GetChild(1).GetChild(0).localScale = Scale;
            }
        }
        [HarmonyPatch(typeof(Nailgun))] static class NailgunH
        {
            static Vector3 Position    = new Vector3(-.0008f, -.0053f, .0003f),
                           Rotation    = new Vector3(0, 180, 0),
                           Scale       = new Vector3(.035f, .035f, .035f),
                           AltPosition = new Vector3(.001f, -.006f, .002f),
                           AltRotation = new Vector3(0, 0, 0),
                           AltScale    = new Vector3(3.5f, 3.5f, 3.5f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Nailgun.Start))] static void AddHand(Nailgun __instance)
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
        [HarmonyPatch(typeof(Railcannon))] static class RailgunH
        {
            static Vector3 Position = new Vector3(-.1f, -.325f, -.025f),
                           Rotation = new Vector3(30, 180, 0),
                           Scale    = new Vector3(350, 350, 350);

            [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))] static void AddHand(Railcannon __instance)
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
}
