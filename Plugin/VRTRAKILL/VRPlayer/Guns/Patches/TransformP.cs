using HarmonyLib;
using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL.VRPlayer.Guns.Patches
{
    [HarmonyPatch] internal class TransformP
    {
        /* Template for transform values:
         static Vector3 Position = new Vector3(),
                        Rotation = new Vector3(),
                        Scale = new Vector3(),
                        AltPosition = new Vector3(),
                        AltRotation = new Vector3(),
                        AltScale = new Vector3();
         */
        [HarmonyPatch(typeof(Revolver))] static class RevolverT
        {
            static Vector3 Position = new Vector3(.05f, -.1f, .6f),
                           Rotation = new Vector3(),
                           Scale = new Vector3(.1f, .1f, .1f),
                           AltPosition = new Vector3(.05f, -.075f, .5f),
                           AltRotation = new Vector3(),
                           AltScale = new Vector3(.085f, .085f, .085f),
                           HandOffsetRotation = new Vector3(0, -90, -90);

            [HarmonyPostfix] [HarmonyPatch(nameof(Revolver.Start))] static void Retransform(Revolver __instance)
            {
                Arms.ArmController.WeaponArmCon WAC = __instance.gameObject.AddComponent<Arms.ArmController.WeaponArmCon>();
                Arm A = Arm.FeedbackerPreset(__instance.transform);
                WAC.SetArm(A); WAC.OffsetRot = HandOffsetRotation;

                if (__instance.altVersion) { ApplyTransform(ref __instance.wpos, AltPosition, AltRotation, AltScale); }
                else { ApplyTransform(ref __instance.wpos, Position, Rotation, Scale); }
            }
        }
        [HarmonyPatch(typeof(Shotgun))] static class ShotgunT
        {
            static Vector3 Position = new Vector3(-.02f, .2f, .26f),
                           Rotation = new Vector3(),
                           Scale = new Vector3(.1f, .1f, .1f),
                           AltPosition = new Vector3(),
                           AltRotation = new Vector3(),
                           AltScale = new Vector3();

            [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))] static void Retransform(Shotgun __instance)
            {
                ApplyTransform(ref __instance.wpos, Position, Rotation, Scale);
            }
        }
        [HarmonyPatch(typeof(Nailgun))] static class NailgunT
        {
            static Vector3 Position = new Vector3(-.165f, .1f, .045f),
                           Rotation = new Vector3(),
                           Scale = new Vector3(.4f, .3275f, .4f),
                           AltPosition = new Vector3(-.165f, .2f, .065f),
                           AltRotation = new Vector3(),
                           AltScale = new Vector3(.35f, .35f, .35f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Nailgun.Start))] static void Retransform(Nailgun __instance)
            {
                if (__instance.altVersion) { ApplyTransform(ref __instance.wpos, AltPosition, AltRotation, AltScale); }
                else { ApplyTransform(ref __instance.wpos, Position, Rotation, Scale); }
            }
        }
        [HarmonyPatch(typeof(Railcannon))] static class RailcannonT
        {
            static Vector3 Position = new Vector3(-.15f, .15f, -.175f);
            static Vector3 Scale = new Vector3(.25f, .25f, .25f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))] static void Retransform(Railcannon __instance)
            {
                __instance.wpos.defaultPos += Position; // what and why?
                __instance.wpos.defaultScale = Scale;
            }
        }
        [HarmonyPatch(typeof(RocketLauncher))] static class RocketLauncherT
        {
            static Vector3 Position = new Vector3(-.3f, .3f, -.1f),
                           Rotation = new Vector3(),
                           Scale = new Vector3(.65f, .65f, .65f),
                           AltPosition = new Vector3(),
                           AltRotation = new Vector3(),
                           AltScale = new Vector3();

            [HarmonyPostfix] [HarmonyPatch(nameof(RocketLauncher.Start))] static void Retransform(RocketLauncher __instance)
            {
                ApplyTransform(__instance.GetComponent<WeaponPos>(), Position, Rotation, Scale);
                if (VRAvatar.VRigController.Instance != null)
                    __instance.transform.GetChild(0).localPosition = Vector3.zero;
            }
        }
        
        [HarmonyPatch(typeof(Sandbox.Arm.SandboxArm))] static class SandboxArmT
        {
            static Vector3 Scale = new Vector3(-.275f, .275f, .275f);

            [HarmonyPostfix] [HarmonyPatch(nameof(Sandbox.Arm.SandboxArm.Awake))] static void Retransform(Sandbox.Arm.SandboxArm __instance)
            {
                //retransform
                Arms.ArmController.DefaultArmCon DAC = __instance.gameObject.AddComponent<Arms.ArmController.DefaultArmCon>();
                Arm A = Arm.SandboxerPreset(__instance.transform);
                DAC.SetArm(A); DAC.OffsetPos = new Vector3(-.15f, -.3f, -.55f);
                __instance.transform.localScale = Scale;
            }
        }

        [HarmonyPatch(typeof(FishingRodWeapon))] static class FishingRodWeaponT
        {
            static Vector3 Position = new Vector3(0, .25f, .3f),
                           Rotation = new Vector3(10, 90, 20),
                           Scale = new Vector3(.1f, .1f, .14f);

            [HarmonyPostfix] [HarmonyPatch(nameof(FishingRodWeapon.Awake))] static void Retransform(FishingRodWeapon __instance)
            {
                ApplyTransform(__instance.GetComponent<WeaponPos>(), Position, Rotation, Scale);

                Arms.ArmController.WeaponArmCon WAC = __instance.gameObject.AddComponent<Arms.ArmController.WeaponArmCon>();
                Arm A = Arm.FeedbackerPreset(__instance.transform);
                WAC.SetArm(A); WAC.OffsetPos = Vector3.zero;
            }
        }

        [HarmonyPrefix] [HarmonyPatch(typeof(WeaponPos), nameof(WeaponPos.Start))] static void Start(WeaponPos __instance)
        {
            __instance.middlePos = __instance.defaultPos;
            __instance.middleRot = __instance.defaultRot;
            __instance.middleScale = __instance.defaultScale;
        }

        public static void ApplyTransform(ref WeaponPos WPos, Vector3 Position = new Vector3(), Vector3 EulerAngles = new Vector3(), Vector3 Scale = new Vector3())
        {
            if (WPos == null) return;
            if (Position != new Vector3())
            {
                WPos.defaultPos = Position; WPos.middlePos = Position;
                WPos.transform.localPosition = Position;
            }
            if (EulerAngles != new Vector3())
            {
                WPos.defaultRot = EulerAngles; WPos.middleRot = EulerAngles;
                WPos.transform.localRotation = Quaternion.Euler(EulerAngles);
            }
            if (Scale != new Vector3())
            {
                WPos.defaultScale = Scale;
                WPos.transform.localScale = Scale;
            }
        }
        public static void ApplyTransform(WeaponPos WPos, Vector3 Position = new Vector3(), Vector3 EulerAngles = new Vector3(), Vector3 Scale = new Vector3())
        {
            if (WPos == null) return;
            if (Position != new Vector3())
            {
                WPos.defaultPos = Position; WPos.middlePos = Position;
                WPos.transform.localPosition = Position;
            }
            if (EulerAngles != new Vector3())
            {
                WPos.defaultRot = EulerAngles; WPos.middleRot = EulerAngles;
                WPos.transform.localRotation = Quaternion.Euler(EulerAngles);
            }
            if (Scale != new Vector3())
            {
                WPos.defaultScale = Scale;
                WPos.transform.localScale = Scale;
            }
        }
    }
}