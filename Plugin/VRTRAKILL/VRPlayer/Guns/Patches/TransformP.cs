using HarmonyLib;
using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] internal class TransformP
    {
        // Applies offset to guns' transforms
        // This took a LOT of time to get those offsets right and my eyes hurt a bit
        [HarmonyPatch(typeof(Revolver))] static class RevolverT
        {
            static Vector3 Position = new Vector3(.05f, -.1f, .6f),
                           AltPosition = new Vector3(.05f, -.075f, .5f),
                           Scale = new Vector3(.1f, .1f, .1f),
                           AltScale = new Vector3(.085f, .085f, .085f),
                           OffsetRotation = new Vector3(0, -90, -90);

            [HarmonyPostfix] [HarmonyPatch(nameof(Revolver.Start))] static void Retransform(Revolver __instance)
            {
                Arms.VRArmsController FBC = __instance.gameObject.AddComponent<Arms.VRArmsController>();
                Arm A = Arm.FeedbackerPreset(__instance.transform);
                FBC.Arm = A; FBC.IsRevolver = true; FBC.OffsetRotation = Quaternion.Euler(OffsetRotation);

                if (__instance.altVersion) { __instance.wpos.defaultPos = AltPosition; __instance.wpos.defaultScale = AltScale; }
                else { __instance.wpos.defaultPos = Position; __instance.wpos.defaultScale = Scale; }
            }
        }
        [HarmonyPatch(typeof(Shotgun))] static class ShotgunT
        {
            static Vector3 Position = new Vector3(0, .2f, .28f);
            static Vector3 Scale = new Vector3(.1f, .1f, .1f); // .1125

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
            static Vector3 Scale = new Vector3(.4f, .4f, .4f),
                           AltScale = new Vector3(.35f, .35f, .35f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Nailgun.Start))] static void Retransform(Nailgun __instance)
            {
                if (__instance.altVersion)
                {
                    __instance.wpos.defaultPos = AltPosition;
                    __instance.wpos.defaultScale = AltScale;
                }
                else
                {
                    __instance.wpos.defaultPos = Position;
                    __instance.wpos.defaultScale = Scale;
                }
                
            }
        }
        [HarmonyPatch(typeof(Railcannon))] static class RailcannonT
        {
            static Vector3 Position = new Vector3(-.25f, .25f, -.25f);
            static Vector3 Scale = new Vector3(.35f, .35f, .35f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))] static void Retransform(Railcannon __instance)
            {
                __instance.wpos.defaultPos += Position;
                __instance.wpos.defaultScale = Scale;
            }
        }
        [HarmonyPatch(typeof(RocketLauncher))] static class RocketLauncherT
        {
            static Vector3 Position = new Vector3(-.3f, .3f, -.1f);
            static Vector3 Scale = new Vector3(.65f, .65f, .65f);

            [HarmonyPostfix] [HarmonyPatch(nameof(RocketLauncher.Start))] static void Retransform(RocketLauncher __instance)
            {
                __instance.gameObject.GetComponent<WeaponPos>().defaultPos = Position;
                __instance.gameObject.GetComponent<WeaponPos>().defaultScale = Scale;
            }
        }
        
        [HarmonyPatch(typeof(Sandbox.Arm.SandboxArm))] static class SandboxArmT
        {
            static Vector3 Scale = new Vector3(-.275f, .275f, .275f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Sandbox.Arm.SandboxArm.Awake))] static void Retransform(Sandbox.Arm.SandboxArm __instance)
            {
                //retransform
                Arms.VRArmsController FBC = __instance.gameObject.AddComponent<Arms.VRArmsController>();
                Arm A = Arm.SandboxerPreset(__instance.transform);
                FBC.Arm = A; FBC.IsSandboxer = true; FBC.OffsetPosition = new Vector3(0, -.25f, -.5f);
                __instance.transform.localScale = Scale;
            }
        }

        [HarmonyPatch(typeof(FishingRodWeapon))] static class FishingRodWeaponT
        {
            static Vector3 Position = new Vector3(0, .25f, .3f);
            static Vector3 Rotation = new Vector3(10, 90, 20);
            static Vector3 Scale = new Vector3(.1f, .1f, .14f);

            [HarmonyPostfix] [HarmonyPatch(nameof(FishingRodWeapon.Awake))] static void Retransform(FishingRodWeapon __instance)
            {
                __instance.gameObject.GetComponent<WeaponPos>().defaultPos = Position;
                __instance.gameObject.GetComponent<WeaponPos>().defaultRot = Rotation;
                __instance.gameObject.GetComponent<WeaponPos>().defaultScale = Scale;
            }
        }

        // Exclude middlepos (or middlepos big gun fix)
        [HarmonyPrefix] [HarmonyPatch(typeof(WeaponPos), nameof(WeaponPos.CheckPosition))] static bool ExcludeMiddlePos(WeaponPos __instance)
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