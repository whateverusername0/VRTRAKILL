using UnityEngine;

namespace Plugin.VRTRAKILL
{
    internal class Vars
    {
        public static Config.ConfigJSON Config = VRTRAKILL.Config.ConfigJSON.Deserialize();

        public static bool IsAMenu =>
               (GameObject.Find("Main Menu State") != null && GameObject.Find("Main Menu State").activeSelf == true)
            || (OptionsManager.Instance != null && OptionsManager.Instance.paused)
            || (SpawnMenu.Instance != null && SpawnMenu.Instance.enabled)
            || (WeaponWheel.Instance != null && WeaponWheel.Instance.enabled)
            || (FinalRank.Instance != null && FinalRank.Instance.enabled)
            || (MenuEsc.current != null);
        public static bool IsSandboxArmActive =>
               Sandbox.Arm.SandboxArm.Instance != null
            && Sandbox.Arm.SandboxArm.Instance.gameObject.activeInHierarchy;

        public static GameObject VRCameraContainer
        { get { return VRPlayer.VRCamera.Patches.CameraConverter.Container; } }

        public static GameObject LeftController
        { get { return VRPlayer.Controllers.LeftArmController.Instance.Offset; } }
        public static GameObject RightController
        { get { return VRPlayer.Controllers.RightArmController.Instance.Offset; } }

        private static Camera _MainCamera; public static Camera MainCamera
        {
            get { if (_MainCamera == null) { _MainCamera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>(); return _MainCamera; }
                  else return _MainCamera; }
        }
    }
}
