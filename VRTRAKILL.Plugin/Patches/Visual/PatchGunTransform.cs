using HarmonyLib;
using UnityEngine;
using Plugin.Systems.VRAvatar.Armature;
using Plugin.Systems.Arms;
using Plugin.Data;

namespace Plugin.Systems.Guns.Patches;

[HarmonyPatch] internal class PatchGunTransform
{
    /* Template for transform values:
     static Vector3
            Position = new Vector3(),
            Rotation = new Vector3(),
            Scale = new Vector3();
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
            VRWeaponArmController WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
            Arm A = Arm.FeedbackerPreset(__instance.transform);
            WAC.Arm = A; WAC.OffsetRot = HandOffsetRotation;

            __instance.wpos.enabled = false;
            if (__instance.altVersion) ApplyTransform(ref __instance.wpos, AltPosition, AltRotation, AltScale);
            else ApplyTransform(ref __instance.wpos, Position, Rotation, Scale);
        }
    }
    [HarmonyPatch(typeof(Shotgun))] static class ShotgunT
    {
        static Vector3
            Position = new Vector3(-.02f, .2f, .26f),
            Rotation = new Vector3(),
            Scale = new Vector3(.1f, .1f, .1f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))]
        static void Retransform(Shotgun __instance)
        {
            ApplyTransform(ref __instance.wpos, Position, Rotation, Scale);
        }

        [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Update))] static void Update(Shotgun __instance)
        {
            if (__instance.anim.GetBool("Sawing"))
                __instance.transform.GetChild(2).localEulerAngles = new Vector3(__instance.transform.GetChild(2).localEulerAngles.x, __instance.transform.GetChild(2).localEulerAngles.y, 25);
            else
                __instance.transform.GetChild(2).localEulerAngles = new Vector3(__instance.transform.GetChild(2).localEulerAngles.x, __instance.transform.GetChild(2).localEulerAngles.y, 0);
        }
    }
    [HarmonyPatch(typeof(ShotgunHammer))] static class ShotgunHammerT
    {
        static Vector3
            Position = new Vector3(0, -.15f, .25f),
            Rotation = new Vector3(),
            Scale = new Vector3(.45f, .45f, .45f);

        [HarmonyPostfix] [HarmonyPatch(nameof(ShotgunHammer.OnEnable))] static void Retransform(ShotgunHammer __instance)
        {
            ApplyTransform(ref __instance.wpos, Position, Rotation, Scale);
            __instance.transform.GetChild(1).localRotation = Quaternion.Euler(0, 180, 0);
            __instance.gameObject.GetComponent<Animator>().enabled = false;
        }
        [HarmonyPostfix] [HarmonyPatch(nameof(ShotgunHammer.LateUpdate))] static void More(ShotgunHammer __instance)
        {
            __instance.transform.GetChild(1).GetChild(0).localPosition = new Vector3(.375f, .415f, .3f);
            __instance.transform.GetChild(1).GetChild(0).localEulerAngles = new Vector3(60,0,0);
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
    [HarmonyPatch(typeof(Chainsaw))] static class ChainsawT
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(Chainsaw.Start))] static void ChainSaw(Chainsaw __instance)
        {
            __instance.lineStartTransform = GunControl.Instance?.currentWeapon != null ? GunControl.Instance?.currentWeapon.transform : Vars.DominantHand.transform;
        }
    }
    
    [HarmonyPatch(typeof(Sandbox.Arm.SandboxArm))] static class SandboxArmT
    {
        static Vector3 Scale = new Vector3(-.275f, .275f, .275f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Sandbox.Arm.SandboxArm.Awake))] static void Retransform(Sandbox.Arm.SandboxArm __instance)
        {
            //retransform
            VRArmController DAC = __instance.gameObject.AddComponent<VRArmController>();
            Arm A = Arm.SandboxerPreset(__instance.transform);
            DAC.Arm = A; DAC.OffsetPos = new Vector3(-.15f, -.3f, -.55f);
            __instance.transform.localScale = Scale;
        }
    }

    [HarmonyPatch(typeof(FishingRodWeapon))] static class FishingRodWeaponT
    {
        static Vector3 Position = new Vector3(.05f, -.06f, -.185f),
                       Rotation = new Vector3(10, 90, 20),
                       Scale = new Vector3(-.0023f, .0023f, .0023f);

        [HarmonyPostfix] [HarmonyPatch(nameof(FishingRodWeapon.Awake))] static void Retransform(FishingRodWeapon __instance)
        {
            ApplyTransform(__instance.GetComponent<WeaponPos>(), Position, Rotation, Scale);

            VRWeaponArmController WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
            Arm A = Arm.FeedbackerPreset(__instance.transform);
            WAC.Arm = A; WAC.OffsetPos = Vector3.zero;
        }
    }
    [HarmonyPatch(typeof(WeaponIdentifier))] static class WasherT
    {
        static Vector3
            Position = new Vector3(0.065f, -0.075f, -0.175f),
            Rotation = new Vector3(10, 90, 20),
            Scale = new Vector3(-.0025f, .0025f, .0025f);
        [HarmonyPostfix] [HarmonyPatch(nameof(WeaponIdentifier.Start))] static void Retransform(WeaponIdentifier __instance)
        {
            var washer = __instance.GetComponentInChildren<Washer>();
            if (washer)
            {
                ApplyTransform(__instance.GetComponent<WeaponPos>(), Position, Rotation, Scale);

                var WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
                var A = Arm.FeedbackerPreset(__instance.transform);
                WAC.Arm = A; WAC.OffsetPos = Vector3.zero;

                washer.defaultSprayPos = new Vector3(.35f, -4.25f, -.675f);
                washer.defaultSprayRot = Quaternion.Euler(90, 0, 0);

                var sprayStart = __instance.GetComponentInChildren<CorrectCameraView>();
                if (sprayStart)
                {
                    sprayStart.transform.localPosition = new Vector3(.35f, -4.25f, -.675f);
                    sprayStart.transform.localEulerAngles = new Vector3(90, 0, 0);
                    sprayStart.enabled = false;
                }
            }
        }
    }
    [HarmonyPatch(typeof(WeaponIdentifier))] static class VacuumT
    {
        static Vector3
            Position = new Vector3(0, -.35f, 2.15f),
            Rotation = new Vector3(0, 90, 20),
            Scale = new Vector3(.125f, .125f, .125f);
        [HarmonyPostfix] [HarmonyPatch(nameof(WeaponIdentifier.Start))] static void Retransform(WeaponIdentifier __instance)
        {
            if (__instance.GetComponentInChildren<Vacuum>())
            {
                ApplyTransform(__instance.GetComponent<WeaponPos>(), Position, Rotation, Scale);

                var WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
                var A = Arm.FeedbackerPreset(__instance.transform);
                WAC.Arm = A; WAC.OffsetPos = Vector3.zero;
            }
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
            WPos.defaultPos = Position; WPos.middlePos = Position; WPos.currentDefault = Position;
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