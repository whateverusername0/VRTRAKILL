using UnityEngine;

namespace Plugin.VRTRAKILL
{
    internal class Vars
    {
        public static Config.ConfigJSON Config = VRTRAKILL.Config.ConfigJSON.Deserialize();

        public static bool IsMainMenu => GameObject.Find("Main Menu State") != null && GameObject.Find("Main Menu State").activeSelf == true;
        public static bool IsPaused => OptionsManager.Instance != null && OptionsManager.Instance.paused;
        public static bool IsSpawnMenuPresent => SpawnMenu.Instance != null && SpawnMenu.Instance.isActiveAndEnabled;
        public static bool IsWeaponWheelPresent => WeaponWheel.Instance != null && WeaponWheel.Instance.isActiveAndEnabled;
        public static bool IsRankingScreenPresent => FinalRank.Instance != null && FinalRank.Instance.isActiveAndEnabled;
        public static bool IsAlterMenuPresent => false; //MenuEsc.current.name == "Sandbox Alter Menu"; // figure out how to detect this

        public static bool IsAMenu =>
               IsMainMenu
            || IsPaused
            || IsSpawnMenuPresent
            || IsWeaponWheelPresent
            || IsRankingScreenPresent
            || IsAlterMenuPresent;

        public static bool IsSandboxArmActive =>
               Sandbox.Arm.SandboxArm.Instance != null
            && Sandbox.Arm.SandboxArm.Instance.isActiveAndEnabled;

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
