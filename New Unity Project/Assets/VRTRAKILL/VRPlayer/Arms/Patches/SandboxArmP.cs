using HarmonyLib;
using UnityEngine;
using Sandbox.Arm;
using ULTRAKILL.Cheats;
using UnityEngine.InputSystem;

namespace Plugin.VRTRAKILL.VRPlayer.Arms.Patches
{
    [HarmonyPatch] internal class SandboxArmP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(SandboxArm), nameof(SandboxArm.Update))] static bool Update(SandboxArm __instance)
        {
            if (Time.timeScale == 0f)
            {
                return false;
            }

            if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame
                && (__instance.currentMode == null || __instance.currentMode.CanOpenMenu))
            {
                __instance.menu.gameObject.SetActive(value: true);
                MonoSingleton<OptionsManager>.Instance.Freeze();
                return false;
            }

            if (!__instance.menu.gameObject.activeSelf)
            {
                if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo()
                    && MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasPerformedThisFrame)
                    __instance.currentMode?.OnPrimaryDown();

                if (MonoSingleton<InputManager>.Instance.InputSource.Fire1.WasCanceledThisFrame)
                    __instance.currentMode?.OnPrimaryUp();

                if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo()
                    && MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame)
                    __instance.currentMode?.OnSecondaryDown();

                if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasCanceledThisFrame)
                    __instance.currentMode?.OnSecondaryUp();
            }

            if (__instance.currentMode != null && __instance.currentMode.Raycast)
            {
                __instance.hitSomething =
                    Physics.Raycast(Vars.DominantHand.transform.position,
                                    Vars.DominantHand.transform.forward,
                                    out __instance.hit,
                                    75f,
                                    __instance.raycastLayers);
            }

            __instance.currentMode?.Update();
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(MoveMode), nameof(MoveMode.Update))] static bool Move(MoveMode __instance)
        {
            __instance.IntegrityCheck();
            if (__instance.manipulatedObject == null) return false;
            if (ExperimentalArmRotation.Enabled)
            {
                Quaternion lhs;
                if (MonoSingleton<InputManager>.Instance.InputSource.ChangeVariation.IsPressed)
                {
                    Vector2 vector = Input.InputVars.TurnVector;
                    lhs = Quaternion.AngleAxis(vector.x * -0.1f, Vector3.up) * Quaternion.AngleAxis(vector.y * 0.1f, Vector3.right);
                    __instance.manipulatedObject.rotationOffset = lhs * __instance.manipulatedObject.rotationOffset;
                }
                __instance.manipulatedObject.target.rotation = Vars.DominantHand.transform.rotation * __instance.manipulatedObject.rotationOffset;
            }
            else
            {
                Vector3 vector2 = new Vector3(__instance.manipulatedObject.originalRotation.x, Vars.DominantHand.transform.eulerAngles.y + __instance.manipulatedObject.simpleRotationOffset, __instance.manipulatedObject.originalRotation.z);
                vector2 = (ULTRAKILL.Cheats.Snapping.SnappingEnabled ? SandboxUtils.SnapRotation(vector2) : vector2);
                __instance.manipulatedObject.target.eulerAngles = vector2;
            }
            if (ExperimentalArmRotation.Enabled)
            {
                __instance.targetPos = Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward * __instance.manipulatedObject.distance;
                if (ULTRAKILL.Cheats.Snapping.SnappingEnabled)
                {
                    __instance.targetPos = SandboxUtils.SnapPos(__instance.targetPos);
                }
                Vector3 a = __instance.targetPos - __instance.manipulatedObject.target.position;
                __instance.manipulatedObject.particles.transform.position = __instance.manipulatedObject.collider.bounds.center;
                __instance.manipulatedObject.target.position += a * 15.5f * Time.deltaTime;
            }
            else
            {
                Vector3 vector3 = new Vector3(__instance.manipulatedObject.originalRotation.x, Vars.DominantHand.transform.eulerAngles.y + __instance.manipulatedObject.simpleRotationOffset, __instance.manipulatedObject.originalRotation.z);
                vector3 = ULTRAKILL.Cheats.Snapping.SnappingEnabled ? SandboxUtils.SnapRotation(vector3) : vector3;
                __instance.manipulatedObject.target.eulerAngles = vector3;
                Vector3 b = Quaternion.Euler(0f, -(__instance.manipulatedObject.originalRotation.y - vector3.y), 0f) * (ULTRAKILL.Cheats.Snapping.SnappingEnabled ? SandboxUtils.SnapPos(__instance.manipulatedObject.positionOffset) : __instance.manipulatedObject.positionOffset);
                Vector3 b2 = __instance.manipulatedObject.target.position - b;
                Vector3 vector4 = Vars.DominantHand.transform.position + Vars.DominantHand.transform.forward * __instance.manipulatedObject.distance;
                __instance.targetPos = vector4;
                if (ULTRAKILL.Cheats.Snapping.SnappingEnabled)
                {
                    vector4 = SandboxUtils.SnapPos(vector4);
                }
                Vector3 a2 = vector4 - b2;
                __instance.manipulatedObject.particles.transform.position = __instance.manipulatedObject.collider.bounds.center;
                __instance.manipulatedObject.target.position += a2 * 15.5f * Time.deltaTime;
            }
            float y = Input.InputVars.TurnVector.y;
            __instance.hostArm.animator.SetFloat(MoveMode.PushZ, y);
            __instance.manipulatedObject.distance += Mathf.Clamp(__instance.manipulatedObject.distance, 1f, 10f) / 10f * y * 0.05f;
            __instance.manipulatedObject.distance = Mathf.Max(0f, __instance.manipulatedObject.distance);
            return false;
        }
        [HarmonyPrefix] [HarmonyPatch(typeof(BuildMode), nameof(BuildMode.Update))] static bool Build(BuildMode __instance)
        {
            if (__instance.tickDelay > 0f) __instance.tickDelay = Mathf.MoveTowards(__instance.tickDelay, 0f, Time.deltaTime);

            Transform transform = Vars.DominantHand.transform;

            bool active = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 75f, __instance.hostArm.raycastLayers);
            if (!__instance.firstBrushPositionSet)
            {
                __instance.pointAIndicatorObject.SetActive(active);
                __instance.pointAIndicatorObject.transform.position = __instance.CalculatePropPosition(hit);
                return false;
            }

            bool flag = Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, 5f, __instance.hostArm.raycastLayers);

            Vector3 vector;
            if (flag) vector = raycastHit.point + new Vector3(0f, 0f, 0f); 
            else vector = transform.position + transform.forward * 4.5f;

            vector = SandboxUtils.SnapPos(vector, __instance.brushOffset, ULTRAKILL.Cheats.Snapping.SnappingEnabled ? 0.5f : 7.5f);

            __instance.pointBIndicatorObject.SetActive(true);
            __instance.pointBIndicatorObject.transform.position = vector;

            if (Mathf.Abs(__instance.firstBlockPos.x - vector.x) >= 1f) __instance.secondBlockPos.x = vector.x; 
            if (Mathf.Abs(__instance.firstBlockPos.y - vector.y) >= 1f) __instance.secondBlockPos.y = vector.y; 
            if (Mathf.Abs(__instance.firstBlockPos.z - vector.z) >= 1f) __instance.secondBlockPos.z = vector.z;

            if (__instance.secondBlockPos != __instance.previousSecondBlockPos)
            {
                if (__instance.tickDelay == 0f)
                {
                    __instance.hostArm.tickSound.pitch = Random.Range(0.74f, 0.76f);
                    __instance.hostArm.tickSound.Play();
                    __instance.tickDelay = 0.05f;
                }
                __instance.previousSecondBlockPos = __instance.secondBlockPos;

                SandboxUtils.SmallerBigger(__instance.firstBlockPos, __instance.secondBlockPos, out Vector3 vector2, out Vector3 a);
                Vector3 size = a - vector2;

                __instance.worldPreviewObject.GetComponent<MeshFilter>().mesh = SandboxUtils.GenerateProceduralMesh(size, true);
                __instance.worldPreviewObject.transform.position = vector2;
            }
            __instance.worldPreviewObject.SetActive(true);
            return false;
        }
    }
}
