using HarmonyLib;
using System.Linq;
using ULTRAKILL.Portal;
using UnityEngine;
using UnityEngine.XR;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.Controllers;
using VRTRAKILL.Systems.Input;

namespace VRTRAKILL.Patches.VRCameraController;

[HarmonyPatch(typeof(CameraController))] internal static class PatchCameraController
{
    public static Camera LeftEye, RightEye;

    private static void SetupEyeCameras(CameraController cc)
    {
        cc.cam.nearClipPlane = 0.01f;
        cc.cam.stereoTargetEye = StereoTargetEyeMask.None;

        // add alwaysontop flag to render arms in world space
        // and disable hud camera since it's no longer necessary
        cc.cam.cullingMask |= 1 << (int)Layers.AlwaysOnTop;
        cc.hudCamera.enabled = false;

        var leftEye = new GameObject("Left", typeof(Camera)).GetComponent<Camera>();
        leftEye.CopyFrom(cc.cam);
        leftEye.transform.SetParent(cc.transform.parent);
        leftEye.stereoTargetEye = StereoTargetEyeMask.Left;
        LeftEye = leftEye;

        var rightEye = new GameObject("Right", typeof(Camera)).GetComponent<Camera>();
        rightEye.CopyFrom(cc.cam);
        rightEye.transform.SetParent(cc.transform.parent);
        rightEye.stereoTargetEye = StereoTargetEyeMask.Right;
        RightEye = rightEye;
    }

    [HarmonyPrefix, HarmonyPatch(nameof(CameraController.Start))]
    private static void Start(CameraController __instance)
    {
        // this is evil because they're all executed on the same thread
        // but it probably works as is and i'm scared to touch it :P
        while (__instance.cam == null && __instance.hudCamera == null) { }

        // for some particular reason destroying it is a bad idea.
        GameObject.Find("Virtual Camera").SetActive(false);

        __instance.cam.enabled = false;

        // TODO add back desktop view in case it's necessary
    }

    [HarmonyPostfix, HarmonyPatch(nameof(CameraController.LateUpdate))]
    static void LateUpdate(CameraController __instance)
    {
        // do nothing
        if (!__instance.player) { __instance.nm = NewMovement.Instance; __instance.player = __instance.nm.gameObject; }

        var vrc = VRCharacterController.Instance;

        __instance.rotationX = -vrc.HeadRotation.eulerAngles.x;
        __instance.rotationY = vrc.HeadRotation.eulerAngles.y + vrc.TurnOffset;
        __instance.tiltRotationZ = vrc.HeadRotation.eulerAngles.z;
        __instance.ApplyRotations();

        var leftd = InputDevices.GetDeviceAtXRNode(XRNode.LeftEye);
        var rightd = InputDevices.GetDeviceAtXRNode(XRNode.RightEye);

        if (leftd.TryGetFeatureValue(CommonUsages.devicePosition, out var lpos))
            PortalAwareSetTransformFromBody(LeftEye.transform, lpos, __instance.transform.rotation);

        if (rightd.TryGetFeatureValue(CommonUsages.devicePosition, out var rpos))
            PortalAwareSetTransformFromBody(RightEye.transform, rpos, __instance.transform.rotation);
    }

    public static void PortalAwareSetTransformFromBody(Transform t, Vector3 localPosition, Quaternion worldRotation, bool hands = false)
    {
        var cc = CameraController.Instance;
        var vrc = VRCharacterController.Instance;

        var parentRot = cc.gravityRotation * Quaternion.AngleAxis(vrc.TurnOffset, Vector3.up);

        var transformedLocalPos = parentRot * localPosition;
        t.position = cc.transform.parent.position + transformedLocalPos;

        if (hands) t.rotation = parentRot * worldRotation;
        else t.rotation = worldRotation;

        MoveFromPlayerThroughPortals(t);
    }

    public static void MoveFromPlayerThroughPortals(Transform t)
    {
        var cc = CameraController.Instance;
        var pm = PortalManagerV2.Instance;
        var pscene = pm.Scene;

        var travellerFlags = PortalTravellerFlags.Player;

        //Code from PortalManager2
        if (pscene.FindCrossedPortal(cc.transform.parent.position, t.transform.position, out var portalHandle, out var intersection))
        {
            var portalObject = pscene.GetPortalObject(portalHandle);
            var travelFlags = portalObject.GetTravelFlags(portalHandle.side);
            var canTravel = travelFlags.HasFlag(travellerFlags);
            var portalSequence = new PortalHandleSequence(new PortalHandle[] { portalHandle });

            if (canTravel)
            {
                var travelMatrix = pscene.GetTravelMatrix(portalHandle);
                var vector2 = travelMatrix.MultiplyPoint3x4(intersection);
                var direction = travelMatrix.MultiplyPoint3x4(t.transform.position) - vector2;
                PortalPhysicsV2.ProjectThroughPortals(vector2, direction, pm.empty_lm, out _, out _, out var intersections);

                for (int i = 0; i < intersections.Length; i++)
                {
                    var portalHandle2 = intersections[i].portalHandle;
                    if (!pscene.GetPortalObject(portalHandle2).GetTravelFlags(intersections[i].portalHandle.side).HasFlag(travellerFlags))
                    {
                        canTravel = false;
                        break;
                    }
                }

                if (canTravel)
                {
                    if (intersections.Length != 0)
                    {
                        portalSequence = PortalHandleSequence.Prepend(portalHandle, intersections);
                        travelMatrix = pscene.GetTravelMatrix(portalSequence);
                    }
                    var details = PortalTravelDetails.WithInteresction(portalSequence, intersections, travelMatrix, intersection);

                    //Actually do the movement
                    t.transform.position = details.enterToExit.MultiplyPoint3x4(t.transform.position);
                    t.transform.rotation = details.enterToExit.rotation * t.transform.rotation;
                }
            }
        }
    }

    [HarmonyPrefix, HarmonyPatch(nameof(CameraController.Transform))]
    private static void TransformPrefix(CameraController __instance, ref float __state)
    {
        __state = __instance.rotationY;
    }

    [HarmonyPostfix, HarmonyPatch(nameof(CameraController.Transform))]
    private static void TransformPostfix(CameraController __instance, ref float __state)
    {
        var vrc = VRCharacterController.Instance;
        vrc.TurnOffset += __instance.rotationY - __state;
    }

    [HarmonyPrefix, HarmonyPatch(nameof(CameraController.GetDefaultPos))]
    private static bool GetDefaultPos(ref Vector3 __result)
    {
        __result = GunsVRController.Instance != null
            ? GlobalVars.DominantHand.transform.position
            : Vector3.zero;
        return false;
    }

    [HarmonyPrefix, HarmonyPatch(typeof(PlayerAnimations), nameof(PlayerAnimations.Start))]
    private static void Start(PlayerAnimations __instance)
    {
        // remove player model
        // TODO reuse player model from uk and avoid using specific asset one

        var head = Object.Instantiate(Assets.VHead, LeftEye.transform);
        head.transform.localPosition = new Vector3(0, 0, -0.07f);
        head.transform.localScale *= 2f;
        if (head.TryGetComponent<CapsuleCollider>(out var cc))
            Object.Destroy(cc);

        foreach (Transform t in head.GetComponentsInChildren<Transform>(true))
            t.gameObject.layer = LayerMask.NameToLayer("Portal");

        // done so we get the correct unlit look
        head.GetComponentInChildren<SkinnedMeshRenderer>().material
            = __instance.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;

        __instance.GetComponentsInChildren<SkinnedMeshRenderer>(true).ToList().ForEach(x => Object.Destroy(x));
        __instance.GetComponentsInChildren<MeshRenderer>(true).ToList().ForEach(x => Object.Destroy(x));
        __instance.GetComponentsInChildren<GunColorGetter>(true).ToList().ForEach(x => Object.Destroy(x));
    }
}
