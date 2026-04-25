using HarmonyLib;
using VRTRAKILL.Data;
using VRTRAKILL.Systems.Controllers;
using UnityEngine;
using Valve.VR;

namespace VRTRAKILL.Patches.Controllers;

[HarmonyPatch(typeof(NewMovement))] internal static partial class PatchNewMovement
{
    [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Start))]
    static void AddControllers(NewMovement __instance)
    {
        __instance.gameObject.SetActive(false);

        var lefthandgo = CreateController("Left Controller", SteamVR_Input_Sources.LeftHand);

        var leftController = lefthandgo.AddComponent<VRControllerController>();
        leftController.RenderModelOffsetPos = new Vector3(.055f, -.1f, -.1f);
        leftController.RenderModelOffsetEulerAngles = new Vector3(75, 0, 0);
        leftController.RenderModelOffsetScale = new Vector3(.65f, .65f, .65f);

        lefthandgo.transform.parent = GlobalVars.VRCameraContainer;

        var righthandgo = CreateController("Right Controller", SteamVR_Input_Sources.RightHand);

        var rightController = righthandgo.AddComponent<VRControllerController>();
        rightController.RenderModelOffsetPos = new Vector3(-.015f, -.105f, -.15f);
        rightController.RenderModelOffsetEulerAngles = new Vector3(75, 0, 0);
        rightController.RenderModelOffsetScale = new Vector3(-.65f, .65f, .65f);

        righthandgo.transform.parent = GlobalVars.VRCameraContainer;

        if (GlobalVars.Config.Controllers.DrawControllers)
        {
            var lefthandmodelgo = CreateControllerModel(SteamVR_Input_Sources.LeftHand, out GameObject _);
            lefthandmodelgo.transform.parent = lefthandgo.transform;

            var righthandmodelgo = CreateControllerModel(SteamVR_Input_Sources.RightHand, out GameObject _);
            righthandmodelgo.transform.parent = righthandgo.transform;
        }

        lefthandgo.AddComponent<ArmsVRController>();
        righthandgo.AddComponent<GunsVRController>();

        __instance.gameObject.SetActive(true);
    }

    static GameObject CreateController(string name, SteamVR_Input_Sources source)
    {
        var go = new GameObject(name) { layer = (int)Layers.IgnoreRaycast };
        var pose = go.AddComponent<SteamVR_Behaviour_Pose>();
        pose.onTransformUpdatedEvent += VRControllerController.onTransformUpdatedH;
        if (source == SteamVR_Input_Sources.LeftHand)
        {
            pose.poseAction = SteamVR_Actions._default.LeftPose;
            pose.inputSource = SteamVR_Input_Sources.LeftHand;
        }
        else if (source == SteamVR_Input_Sources.RightHand)
        {
            pose.poseAction = SteamVR_Actions._default.RightPose;
            pose.inputSource = SteamVR_Input_Sources.RightHand;
        }
        else throw new System.NotImplementedException();
        return go;
    }

    static GameObject CreateControllerModel(SteamVR_Input_Sources source, out GameObject sandboxRM)
    {
        var go = new GameObject("model") { layer = (int)Layers.IgnoreRaycast };
        sandboxRM = null;

        Transform T;
        if (source == SteamVR_Input_Sources.LeftHand)
        {
            T = Object.Instantiate(Assets.Controller_ND).transform;
            T.parent = go.transform;
            T.localPosition = Vector3.zero;
        }
        else if (source == SteamVR_Input_Sources.RightHand)
        {

            T = Object.Instantiate(Assets.Controller_D).transform;
            sandboxRM = Object.Instantiate(Assets.Controller_D_Sandbox);
            T.parent = go.transform;
            T.localPosition = Vector3.zero;
        }
        else throw new System.NotImplementedException();

        if (sandboxRM != null)
        {
            sandboxRM.transform.parent = go.transform;
            sandboxRM.transform.localPosition = Vector3.zero;
        }

        return go;
    }
}
