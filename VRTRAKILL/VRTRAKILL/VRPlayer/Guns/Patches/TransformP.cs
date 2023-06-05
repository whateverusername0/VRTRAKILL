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
            static Vector3 Position = new Vector3(.05f, -.1f, .6f),
                           AltPosition = new Vector3(.05f, -.075f, .575f);
            static Vector3 Scale = new Vector3(.1f, .1f, .1f),
                           AltScale = new Vector3(.105f, .105f, .105f);

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
            static Vector3 MovePosition    = new Vector3(0, -.1f, -.25f),
                           AlterPosition   = new Vector3(0, -.15f, -.2f),
                           BuildPosition   = new Vector3(0, -.1f, -.3f),
                           PlacePosition   = new Vector3(0, -.1f, -.3f);
            static Vector3 OffsetRotation = new Vector3(0, 0, 0),
                           PlaceOffsetRotation = new Vector3(0, 0, 90);
            static Vector3 Scale = new Vector3(-.35f, .35f, .35f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Sandbox.Arm.SandboxArm.OnEnable))] static void Retransform(Sandbox.Arm.SandboxArm __instance)
            {
                switch (__instance.currentMode.Name)
                {
                    case "Move":
                        __instance.transform.localPosition = MovePosition;
                        __instance.transform.rotation = Vars.RightController.transform.rotation * Quaternion.Euler(-OffsetRotation);
                        __instance.transform.localScale = Scale;
                        break;
                    case "Destroy":
                    case "Alter":
                        __instance.transform.localPosition = AlterPosition;
                        __instance.transform.rotation = Vars.RightController.transform.rotation * Quaternion.Euler(-OffsetRotation);
                        __instance.transform.localScale = Scale;
                        break;
                    case "Build":
                    case "Place":
                        __instance.transform.localPosition = PlacePosition;
                        __instance.transform.rotation = Vars.RightController.transform.rotation * Quaternion.Euler(PlaceOffsetRotation);
                        __instance.transform.localScale = Scale;
                        break;
                }
            }
        }

        // Exclude middlepos (or middlepos big gun fix)
        [HarmonyPatch(typeof(WeaponPos), nameof(WeaponPos.CheckPosition))] static bool ExcludeMiddlePos(WeaponPos __instance)
        {
            if (!__instance.ready)
            {
                __instance.ready = true;
                __instance.defaultPos = __instance.transform.localPosition;
                __instance.defaultRot = __instance.transform.localRotation.eulerAngles;
                __instance.defaultScale = __instance.transform.localScale;

                if (__instance.middleScale == Vector3.zero) __instance.middleScale = __instance.defaultScale;
                if (__instance.middleRot == Vector3.zero) __instance.middleRot = __instance.defaultRot;
                //if (__instance.moveOnMiddlePos != null && __instance.moveOnMiddlePos.Length != 0)
                //{
                //    for (int i = 0; i < __instance.moveOnMiddlePos.Length; i++)
                //    {
                //        __instance.defaultPosValues.Add(__instance.moveOnMiddlePos[i].localPosition);
                //        __instance.defaultRotValues.Add(__instance.moveOnMiddlePos[i].localEulerAngles);
                //        if (__instance.middleRotValues[i] == Vector3.zero)
                //            __instance.middleRotValues[i] = __instance.moveOnMiddlePos[i].localEulerAngles;
                //    }
                //}
            }
            //if (MonoSingleton<PrefsManager>.Instance.GetInt("weaponHoldPosition", 0) == 1 && (!MonoSingleton<PowerUpMeter>.Instance || MonoSingleton<PowerUpMeter>.Instance.juice <= 0f))
            //{
            //    __instance.transform.localPosition = __instance.middlePos;
            //    __instance.transform.localRotation = Quaternion.Euler(__instance.middleRot);
            //    __instance.transform.localScale = __instance.middleScale;
            //    if (__instance.moveOnMiddlePos != null && __instance.moveOnMiddlePos.Length != 0)
            //    {
            //        for (int j = 0; j < __instance.moveOnMiddlePos.Length; j++)
            //        {
            //            __instance.moveOnMiddlePos[j].localPosition = __instance.middlePosValues[j];
            //            __instance.moveOnMiddlePos[j].localEulerAngles = __instance.middleRotValues[j];
            //        }
            //    }
            //}
            //else
            //{
                __instance.transform.localPosition = __instance.defaultPos;
                __instance.transform.localRotation = Quaternion.Euler(__instance.defaultRot);
                __instance.transform.localScale = __instance.defaultScale;
                //if (__instance.moveOnMiddlePos != null && __instance.moveOnMiddlePos.Length != 0)
                //{
                //    for (int k = 0; k < __instance.moveOnMiddlePos.Length; k++)
                //    {
                //        __instance.moveOnMiddlePos[k].localPosition = __instance.defaultPosValues[k];
                //        __instance.moveOnMiddlePos[k].localEulerAngles = __instance.defaultRotValues[k];
                //    }
                //}
            //}
            __instance.currentDefault = __instance.transform.localPosition;
            return false;
        }
    }
}