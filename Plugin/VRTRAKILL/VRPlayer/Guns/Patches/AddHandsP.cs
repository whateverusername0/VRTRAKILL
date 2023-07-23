using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] internal class AddHandsP
    {
        // here small point-perfect numbers MATTER. :(
        [HarmonyPatch(typeof(Shotgun))] static class ShotgunH
        {
            static Vector3 Position = new Vector3(-18.2475f, 26.3511f, 49.656f),
                           Rotation = new Vector3(0, 270, 270);

            [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))] static void AddHand(Shotgun __instance)
            {
                Assets.Vars.HandPose_Shotgun.transform.position = Vector3.zero;

                // Shotgun ******(Clone)/Shotgun_New/GunArmature/MainBone
                Assets.Vars.HandPose_Shotgun.transform
                    .SetParent(__instance.transform.GetChild(2).GetChild(0), false);

                Assets.Vars.HandPose_Shotgun.transform.localPosition = Position;
                Assets.Vars.HandPose_Shotgun.transform.localEulerAngles = Rotation;
            }
        }
        [HarmonyPatch(typeof(Nailgun))] static class NailgunH
        {
            static Vector3 Position = new Vector3(-.0004f, -.002f, -.0008f),
                           Rotation = new Vector3(0, 90, 270),
                           AltPosition = new Vector3(3, -25, -24), AltHandPosition = new Vector3(-.083f, .699f, .69f),
                           AltRotation = Vector3.zero;

            [HarmonyPostfix] [HarmonyPatch(nameof(Nailgun.Start))] static void AddHand(Nailgun __instance)
            {
                Assets.Vars.HandPose_Nailgun.transform.position = Vector3.zero;

                // Nailgun ******(Clone)/Nailgun New New/Armature/Main
                // Sawblade Launcher ******(Clone)/Sawblade Launcher/Armature/Base
                Assets.Vars.HandPose_Nailgun.transform
                        .SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);

                if (__instance.altVersion)
                {
                    Assets.Vars.HandPose_Nailgun.transform.localPosition = AltPosition;
                    Assets.Vars.HandPose_Nailgun.transform.localEulerAngles = AltRotation;
                    Assets.Vars.HandPose_Nailgun.transform.localScale = new Vector3(35, 35, 35);

                    Assets.Vars.HandPose_Nailgun.transform.GetChild(1).localPosition = AltHandPosition;
                    Assets.Vars.HandPose_Nailgun.transform.GetChild(1).localEulerAngles = Vector3.zero;
                }
                else
                {
                    Assets.Vars.HandPose_Nailgun.transform
                        .SetParent(__instance.transform.GetChild(0).GetChild(0).GetChild(0), false);

                    Assets.Vars.HandPose_Nailgun.transform.localPosition = Position;
                    Assets.Vars.HandPose_Nailgun.transform.localEulerAngles = Rotation;
                    Assets.Vars.HandPose_Nailgun.transform.localScale = new Vector3(.3f, -.3f, -.3f);
                }
            }
        }
        [HarmonyPatch(typeof(Railcannon))] static class RailgunH
        {
            static Vector3 Position = new Vector3(),
                           Rotation = new Vector3();

            [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))] static void AddHand(Railcannon __instance)
            {
                // blehh
                //Assets.Vars.HandPose_Railgun
            }
        }
    }
}
