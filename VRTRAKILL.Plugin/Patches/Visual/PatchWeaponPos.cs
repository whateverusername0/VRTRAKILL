using HarmonyLib;
using UnityEngine;
using VRTRAKILL.Systems.VRAvatar.Armature;
using VRTRAKILL.Systems.Arms;
using VRTRAKILL.Data;

namespace VRTRAKILL.Systems.Guns.Patches;

[HarmonyPatch] internal class PatchWeaponPos
{
    /* Template for transform values:
     static Vector3
            Position = new(),
            Rotation = new(),
            Scale = new();
     */
    [HarmonyPatch(typeof(Revolver))] internal class TransformRevolver
    {
        static Vector3 Position = new(.05f, -.1f, .6f),
                       Rotation = new(),
                       Scale = new(.1f, .1f, .1f),
                       AltPosition = new(.05f, -.075f, .5f),
                       AltRotation = new(),
                       AltScale = new(.085f, .085f, .085f),
                       HandOffsetRotation = new(0, -90, -90);

        [HarmonyPostfix] [HarmonyPatch(nameof(Revolver.Start))]
        private static void Start(Revolver __instance)
        {
            VRWeaponArmController WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
            Arm A = Arm.FeedbackerPreset(__instance.transform);
            WAC.Arm = A; WAC.OffsetRot = HandOffsetRotation;

            __instance.wpos.enabled = false;
            if (__instance.altVersion) ApplyTransform(ref __instance.wpos, AltPosition, AltRotation, AltScale);
            else ApplyTransform(ref __instance.wpos, Position, Rotation, Scale);
        }
    }
    [HarmonyPatch(typeof(Shotgun))] internal class TransformShotgun
    {
        static Vector3
            Position = new(-.02f, .2f, .26f),
            Rotation = new(),
            Scale = new(.1f, .1f, .1f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Start))]
        private static void Start(Shotgun __instance)
        {
            ApplyTransform(ref __instance.wpos, Position, Rotation, Scale);
        }

        [HarmonyPostfix] [HarmonyPatch(nameof(Shotgun.Update))]
        private static void Update(Shotgun __instance)
        {
            if (__instance.anim.GetBool("Sawing"))
                __instance.transform.GetChild(2).localEulerAngles = new(__instance.transform.GetChild(2).localEulerAngles.x, __instance.transform.GetChild(2).localEulerAngles.y, 25);
            else
                __instance.transform.GetChild(2).localEulerAngles = new(__instance.transform.GetChild(2).localEulerAngles.x, __instance.transform.GetChild(2).localEulerAngles.y, 0);
        }
    }
    [HarmonyPatch(typeof(ShotgunHammer))] internal class TransformShotgunHammer
    {
        static Vector3
            Position = new(0, -.15f, .25f),
            Rotation = new(),
            Scale = new(.45f, .45f, .45f);

        [HarmonyPostfix] [HarmonyPatch(nameof(ShotgunHammer.OnEnable))]
        private static void Start(ShotgunHammer __instance)
        {
            ApplyTransform(ref __instance.wpos, Position, Rotation, Scale);
            __instance.transform.GetChild(1).localRotation = Quaternion.Euler(0, 180, 0);
            __instance.gameObject.GetComponent<Animator>().enabled = false;
        }

        [HarmonyPostfix] [HarmonyPatch(nameof(ShotgunHammer.LateUpdate))]
        private static void More(ShotgunHammer __instance)
        {
            __instance.transform.GetChild(1).GetChild(0).localPosition = new(.375f, .415f, .3f);
            __instance.transform.GetChild(1).GetChild(0).localEulerAngles = new(60,0,0);
        }
    }
    [HarmonyPatch(typeof(Nailgun))] internal class TransformNailgun
    {
        static Vector3 Position = new(-.165f, .1f, .045f),
                       Rotation = new(),
                       Scale = new(.4f, .3275f, .4f),
                       AltPosition = new(-.165f, .2f, .065f),
                       AltRotation = new(),
                       AltScale = new(.35f, .35f, .35f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Nailgun.Start))]
        private static void Start(Nailgun __instance)
        {
            if (__instance.altVersion) { ApplyTransform(ref __instance.wpos, AltPosition, AltRotation, AltScale); }
            else { ApplyTransform(ref __instance.wpos, Position, Rotation, Scale); }
        }
    }
    [HarmonyPatch(typeof(Railcannon))] internal class TransformRailcannon
    {
        static Vector3 Position = new(-.15f, .15f, -.175f);
        static Vector3 Scale = new(.25f, .25f, .25f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Railcannon.Start))]
        private static void Start(Railcannon __instance)
        {
            __instance.wpos.defaultPos += Position; // what and why?
            __instance.wpos.defaultScale = Scale;
        }
    }
    [HarmonyPatch(typeof(RocketLauncher))] internal class TransformRocketLauncher
    {
        static Vector3 Position = new(-.3f, .3f, -.1f),
                       Rotation = new(),
                       Scale = new(.65f, .65f, .65f),
                       AltPosition = new(),
                       AltRotation = new(),
                       AltScale = new();

        [HarmonyPostfix] [HarmonyPatch(nameof(RocketLauncher.Start))]
        private static void Start(RocketLauncher __instance)
        {
            ApplyTransform(__instance.GetComponent<WeaponPos>(), Position, Rotation, Scale);
            if (VRAvatar.VRigController.Instance != null)
                __instance.transform.GetChild(0).localPosition = Vector3.zero;
        }
    }
    [HarmonyPatch(typeof(Chainsaw))] internal class TransformChainsaw
    {
        [HarmonyPostfix] [HarmonyPatch(nameof(Chainsaw.Start))]
        private static void ChainSaw(Chainsaw __instance)
        {
            __instance.lineStartTransform = GunControl.Instance?.currentWeapon != null ? GunControl.Instance?.currentWeapon.transform : Vars.DominantHand.transform;
        }
    }
    
    [HarmonyPatch(typeof(Sandbox.Arm.SandboxArm))] internal class TransformSandboxArm
    {
        static Vector3 Scale = new(-.275f, .275f, .275f);

        [HarmonyPostfix] [HarmonyPatch(nameof(Sandbox.Arm.SandboxArm.Awake))]
        private static void Start(Sandbox.Arm.SandboxArm __instance)
        {
            //retransform
            VRArmController DAC = __instance.gameObject.AddComponent<VRArmController>();
            Arm A = Arm.SandboxerPreset(__instance.transform);
            DAC.Arm = A; DAC.OffsetPos = new(-.15f, -.3f, -.55f);
            __instance.transform.localScale = Scale;
        }
    }

    [HarmonyPatch(typeof(FishingRodWeapon))] internal class TransformFishingRodWeapon
    {
        static Vector3 Position = new(.05f, -.06f, -.185f),
                       Rotation = new(10, 90, 20),
                       Scale = new(-.0023f, .0023f, .0023f);

        [HarmonyPostfix] [HarmonyPatch(nameof(FishingRodWeapon.Awake))]
        private static void Start(FishingRodWeapon __instance)
        {
            ApplyTransform(__instance.GetComponent<WeaponPos>(), Position, Rotation, Scale);

            VRWeaponArmController WAC = __instance.gameObject.AddComponent<VRWeaponArmController>();
            Arm A = Arm.FeedbackerPreset(__instance.transform);
            WAC.Arm = A; WAC.OffsetPos = Vector3.zero;
        }
    }
    [HarmonyPatch(typeof(WeaponIdentifier))] internal class TransformWasher
    {
        static Vector3
            Position = new(0.065f, -0.075f, -0.175f),
            Rotation = new(10, 90, 20),
            Scale = new(-.0025f, .0025f, .0025f);

        [HarmonyPostfix] [HarmonyPatch(nameof(WeaponIdentifier.Start))]
        private static void Start(WeaponIdentifier __instance)
        {
            var washer = __instance.GetComponentInChildren<Washer>();
            if (washer)
            {
                ApplyTransform(__instance.GetComponent<WeaponPos>(), Position, Rotation, Scale);

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
            }
        }
    }
    [HarmonyPatch(typeof(WeaponIdentifier))] internal class TransformVacuum
    {
        static Vector3
            Position = new(0, -.35f, 2.15f),
            Rotation = new(0, 90, 20),
            Scale = new(.125f, .125f, .125f);

        [HarmonyPostfix] [HarmonyPatch(nameof(WeaponIdentifier.Start))]
        private static void Start(WeaponIdentifier __instance)
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

    [HarmonyPrefix] [HarmonyPatch(typeof(WeaponPos), nameof(WeaponPos.Start))]
    private static void Start(WeaponPos __instance)
    {
        __instance.middlePos = __instance.defaultPos;
        __instance.middleRot = __instance.defaultRot;
        __instance.middleScale = __instance.defaultScale;
    }

    protected static void ApplyTransform(ref WeaponPos WPos, Vector3 Position = new(), Vector3 EulerAngles = new(), Vector3 Scale = new())
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

    protected static void ApplyTransform(WeaponPos WPos, Vector3 Position = new(), Vector3 EulerAngles = new(), Vector3 Scale = new())
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