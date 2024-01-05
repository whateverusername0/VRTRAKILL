using BepInEx;
using UnityEngine;
using VRBasePlugin.ULTRAKILL;
using HarmonyLib;
using VRBasePlugin.ULTRAKILL.VRPlayer.VRCamera.Patches;
using VRBasePlugin.ULTRAKILL.VRPlayer;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Goodies.ThirdPersonCamera
{
    [BepInDependency(VRBasePlugin.PluginInfo.PLUGIN_GUID, BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Camera ThirdPersonCam { get; set; }

        public void Awake()
        {
            new Patcher(new Harmony(PluginInfo.PLUGIN_GUID)).PatchAll();
            Logger.LogInfo("Successfully loaded.");
        }
    }

    [HarmonyPatch] static class Patches
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CameraConverterP), nameof(CameraConverterP.Containerize))]
        public static void AddTPC()
        {
            GameObject TPCContainer = new GameObject("Third Person Camera");
            TPCContainer.transform.localPosition = Vector3.zero;

            GameObject TPCOffset = new GameObject("TPC Limiter");
            TPCOffset.transform.parent = TPCContainer.transform;
            TPCOffset.transform.localPosition = Vector3.zero;

            Plugin.ThirdPersonCam = new GameObject("TPC Camera").AddComponent<Camera>();
            Plugin.ThirdPersonCam.transform.parent = TPCContainer.transform;
            Plugin.ThirdPersonCam.transform.localPosition = Vector3.zero;
            Plugin.ThirdPersonCam.stereoTargetEye = StereoTargetEyeMask.None;

            ThirdPersonCamera TPC = TPCContainer.AddComponent<ThirdPersonCamera>();
            TPC.TPCam = Plugin.ThirdPersonCam;
            TPC.FollowTarget = Vars.MainCamera.transform;
            TPC.Offset = TPCOffset.transform;

            // Pushback 2.0
            Rigidbody TPCRB = Plugin.ThirdPersonCam.gameObject.AddComponent<Rigidbody>();
            TPCRB.mass = 0; TPCRB.drag = 0; TPCRB.angularDrag = 0; TPCRB.useGravity = false;
            TPCRB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            TPCRB.constraints = RigidbodyConstraints.FreezeRotation;
            SphereCollider TTPC = Plugin.ThirdPersonCam.gameObject.AddComponent<SphereCollider>(); TTPC.radius = .01f;
            TPC.RB = TPCRB;

            TPCContainer.SetActive(false);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(VRKeybindsController), nameof(VRKeybindsController.Start))]
        static void AddController()
        {
            if (Vars.Config.DesktopView.Enabled)
            {
                if (Vars.Config.DesktopView.ThirdPersonCamera.Enabled)
                    Plugin.ThirdPersonCam.gameObject.SetActive(true);
                else Vars.DesktopCamera.gameObject.SetActive(true);
                Vars.DesktopUICamera.gameObject.SetActive(true);
            }

            if (ThirdPersonCamera.Instance != null)
                if (Vars.Config.DesktopView.ThirdPersonCamera.Mode <= 2 || Vars.Config.DesktopView.ThirdPersonCamera.Mode >= 0)
                    ThirdPersonCamera.Instance.Mode = (TPCMode)Vars.Config.DesktopView.ThirdPersonCamera.Mode;
                else ThirdPersonCamera.Instance.Mode = 0;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(VRKeybindsController), nameof(VRKeybindsController.Update))]
        static void AddListener()
        {
            if (UnityEngine.Input.GetKeyDown((KeyCode)VRBasePlugin.ULTRAKILL.Config.ConfigMaster.ToggleThirdPersonCamera))
            {
                SubtitleController.Instance.DisplaySubtitle("VR: Toggling third person camera");
                ToggleSpectatorCamera();
            }
            if (UnityEngine.Input.GetKeyDown((KeyCode)VRBasePlugin.ULTRAKILL.Config.ConfigMaster.EnumTPCamMode))
            {
                ThirdPersonCamera.Instance.EnumTPCMode();
                SubtitleController.Instance.DisplaySubtitle
                    ($"VR: Switched third person camera mode to {System.Enum.GetName(typeof(TPCMode), ThirdPersonCamera.Instance.Mode)}");
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(VRKeybindsController), nameof(VRKeybindsController.ToggleDesktopView))]
        static void OverwriteBehavior()
        {
            Plugin.ThirdPersonCam.gameObject.SetActive(false);

            if (!Vars.DesktopCamera.gameObject.activeSelf) Vars.DesktopCamera.gameObject.SetActive(true);
            else Vars.DesktopCamera.gameObject.SetActive(false);

            if (!Vars.DesktopUICamera.gameObject.activeSelf) Vars.DesktopUICamera.gameObject.SetActive(true);
            else if (Vars.DesktopUICamera.gameObject.activeSelf && !Plugin.ThirdPersonCam.gameObject.activeSelf)
                Vars.DesktopUICamera.gameObject.SetActive(false);
        }

        private static void ToggleSpectatorCamera()
        {
            if (!Plugin.ThirdPersonCam.gameObject.activeSelf)
            {
                if (Vars.DesktopCamera.gameObject.activeSelf)
                {
                    VRKeybindsController.WasDVActive = true;
                    Vars.DesktopCamera.gameObject.SetActive(false);
                }
                Plugin.ThirdPersonCam.gameObject.SetActive(true);

                if (!Vars.DesktopUICamera.gameObject.activeSelf)
                    Vars.DesktopUICamera.gameObject.SetActive(true);
            }
            else
            {
                if (VRKeybindsController.WasDVActive)
                {
                    Vars.DesktopCamera.gameObject.SetActive(true);
                    VRKeybindsController.WasDVActive = false;
                }
                else Vars.DesktopUICamera.gameObject.SetActive(false);
                Plugin.ThirdPersonCam.gameObject.gameObject.SetActive(false);
            }
        }
    }
}
