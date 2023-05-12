using HarmonyLib;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] internal class TransformP
    {
        // Applies offset to guns' transforms
        // This took a LOT of time to get those offsets right and my eyes hurt a bit
        [HarmonyPatch(typeof(Revolver))] static class RevolverT
        {
            static Vector3 Position = new Vector3(0, -.1f, .6f),
                           AltPosition = new Vector3(0, -.075f, .575f);
            static Vector3 Scale = new Vector3(.1f, .1f, .1f),
                           AltScale = new Vector3(.1f, .1f, .1f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Revolver.Start))] static void Retransform(Revolver __instance)
            {
                if (__instance.altVersion == true) { __instance.wpos.defaultPos = AltPosition; __instance.wpos.defaultScale = AltScale; }
                else { __instance.wpos.defaultPos = Position; __instance.wpos.defaultScale = Scale; }
            }
        }
        [HarmonyPatch(typeof(Shotgun))] static class ShotgunT
        {
            static Vector3 Position = new Vector3(0, .2f, .28f);
            static Vector3 Scale = new Vector3(.1125f, .1125f, .1125f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))] static void Retransform(Shotgun __instance)
            {
                __instance.wpos.defaultPos = Position;
                __instance.wpos.defaultScale = Scale;
            }
        }
        [HarmonyPatch(typeof(Nailgun))] static class NailgunT
        {
            static Vector3 Position = new Vector3(-.2f, .2125f, -.05f),
                           AltPosition = new Vector3(-.25f, .35f, .075f);
            static Vector3 Scale = new Vector3(.5f, .5f, .5f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Nailgun.Start))] static void Retransform(Nailgun __instance)
            {
                if (__instance.altVersion == true) __instance.wpos.defaultPos = AltPosition;
                else __instance.wpos.defaultPos = Position;
                __instance.wpos.defaultScale = Scale;
            }
        }
        [HarmonyPatch(typeof(Railcannon))] static class RailcannonT
        {
            static Vector3 Position = new Vector3(-.25f, .25f, -.25f);
            static Vector3 Scale = new Vector3(.4f, .4f, .4f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))] static void Retransform(Railcannon __instance)
            {
                __instance.wpos.defaultPos += Position;
                __instance.wpos.defaultScale = Scale;
            }
        }
        [HarmonyPatch(typeof(RocketLauncher))] static class RocketLauncherT
        {
            static Vector3 Position = new Vector3(-.3f, .3f, -.1f);
            static Vector3 Scale = new Vector3(.75f, .75f, .75f);

            [HarmonyPostfix] [HarmonyPatch(nameof(RocketLauncher.Start))] static void Retransform(RocketLauncher __instance)
            {
                __instance.gameObject.GetComponent<WeaponPos>().defaultPos = Position;
                __instance.gameObject.GetComponent<WeaponPos>().defaultScale = Scale;
            }
        }
        
        [HarmonyPatch(typeof(Sandbox.Arm.SandboxArm))] static class SandboxArmT
        {
            static Vector3 MovePosition    = new Vector3(-.15f, .2f, -1.05f),
                           DestroyPosition = new Vector3(.05f, -.075f, -.8f),
                           AlterPosition   = new Vector3(.05f, -.075f, -.8f),
                           BuildPosition   = new Vector3(0, .25f, -1.2f),
                           PlacePosition   = new Vector3(0, .25f, -1.2f);
            static Vector3 Scale = new Vector3(-.35f, .35f, .35f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Sandbox.Arm.SandboxArm.OnEnable))] static void Retransform(Sandbox.Arm.SandboxArm __instance)
            {
                switch (__instance.currentMode.Name)
                {
                    case "Move":
                        __instance.transform.localPosition = MovePosition;
                        __instance.transform.localScale = Scale;
                        break;
                    case "Destroy":
                        __instance.transform.localPosition = DestroyPosition;
                        __instance.transform.localScale = Scale;
                        break;
                    case "Alter":
                        __instance.transform.localPosition = AlterPosition;
                        __instance.transform.localScale = Scale;
                        break;
                    case "Build":
                        __instance.transform.localPosition = BuildPosition;
                        __instance.transform.localScale = Scale;
                        break;
                    case "Place":
                        __instance.transform.localPosition = PlacePosition;
                        __instance.transform.localScale = Scale;
                        break;
                }
            }
        }
    }
}