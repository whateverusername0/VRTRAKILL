using HarmonyLib;
using VRTRAKILL.Data;
using VRTRAKILL.Systems;
using VRTRAKILL.Systems.Controllers;
using UnityEngine;
using Valve.VR;

namespace VRTRAKILL.Patches.Controllers;

[HarmonyPatch(typeof(NewMovement))] internal sealed class PatchControllerAdder
{
    [HarmonyPostfix] [HarmonyPatch(nameof(NewMovement.Start))] public static void AddHands(NewMovement __instance)
    {
        __instance.gameObject.SetActive(false);

        GameObject LHGO = CreateController("Left Controller", SteamVR_Input_Sources.LeftHand);

        VRControllersSystem LCon = LHGO.AddComponent<VRControllersSystem>();
        LCon.RenderModelOffsetPos = new Vector3(.055f, -.1f, -.1f);
        LCon.RenderModelOffsetEulerAngles = new Vector3(75, 0, 0);
        LCon.RenderModelOffsetScale = new Vector3(.65f, .65f, .65f);

        LHGO.transform.parent = Vars.VRCameraContainer;

        GameObject RHGO = CreateController("Right Controller", SteamVR_Input_Sources.RightHand);

        VRControllersSystem RCon = RHGO.AddComponent<VRControllersSystem>();
        RCon.RenderModelOffsetPos = new Vector3(-.015f, -.105f, -.15f);
        RCon.RenderModelOffsetEulerAngles = new Vector3(75, 0, 0);
        RCon.RenderModelOffsetScale = new Vector3(-.65f, .65f, .65f);

        RHGO.transform.parent = Vars.VRCameraContainer;

        if (Vars.Config.Controllers.DrawControllers)
        {
            GameObject LHMGO = CreateControllerModel(SteamVR_Input_Sources.LeftHand, out GameObject _);
            LHMGO.transform.parent = LHGO.transform;

            GameObject RHMGO = CreateControllerModel(SteamVR_Input_Sources.RightHand, out GameObject _);
            RHMGO.transform.parent = RHGO.transform;
        }

        LHGO.AddComponent<VRArmsSystem>();
        RHGO.AddComponent<VRGunsSystem>();

        __instance.gameObject.SetActive(true);
    }

    private static GameObject CreateController(string Name, SteamVR_Input_Sources Source)
    {
        GameObject GO = new GameObject(Name) { layer = (int)Layers.IgnoreRaycast };
        SteamVR_Behaviour_Pose Controller = GO.AddComponent<SteamVR_Behaviour_Pose>();
        Controller.onTransformUpdatedEvent += VRControllersSystem.onTransformUpdatedH;
        if (Source == SteamVR_Input_Sources.LeftHand)
        {
            Controller.poseAction = SteamVR_Actions._default.LeftPose;
            Controller.inputSource = SteamVR_Input_Sources.LeftHand;
        }
        else if (Source == SteamVR_Input_Sources.RightHand)
        {
            Controller.poseAction = SteamVR_Actions._default.RightPose;
            Controller.inputSource = SteamVR_Input_Sources.RightHand;
        }
        else throw new System.NotImplementedException();
        return GO;
    }
    private static GameObject CreateControllerModel(SteamVR_Input_Sources Source, out GameObject SandboxRM, string Name = "Model")
    {
        GameObject GO = new GameObject(Name) { layer = (int)Layers.IgnoreRaycast };
        SandboxRM = null;

        Transform T;
        if (Source == SteamVR_Input_Sources.LeftHand)
        {
            T = Object.Instantiate(Assets.Controller_ND).transform;
            T.parent = GO.transform;
            T.localPosition = Vector3.zero;
        }
        else if (Source == SteamVR_Input_Sources.RightHand)
        {

            T = Object.Instantiate(Assets.Controller_D).transform;
            SandboxRM = Object.Instantiate(Assets.Controller_D_Sandbox);
            T.parent = GO.transform;
            T.localPosition = Vector3.zero;
        }
        else throw new System.NotImplementedException();

        if (SandboxRM != null)
        {
            SandboxRM.transform.parent = GO.transform;
            SandboxRM.transform.localPosition = Vector3.zero;
        }

        return GO;
    }
}
