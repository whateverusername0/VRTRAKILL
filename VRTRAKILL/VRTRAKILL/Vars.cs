using UnityEngine;

namespace Plugin.VRTRAKILL
{
    internal class Vars
    {
        public enum Layers
        {
            Default = 0,
            TransparentFX = 1,
            IgnoreRaycast = 2,
            Water = 4,
            UI = 5,
            Environment = 8,
            Gib = 9, Limb = 10, BigCorpse = 11,
            EnemyTrigger = 12, AlwaysOnTop = 13,
            Projectile = 14,
            Invincible = 15, Invisible = 16,
            BrokenGlass = 17,
            PlayerOnly = 18,
            VirtualScreen = 19,
            GroundCheck = 20,
            EnemyWall = 21,
            Item = 22,
            Explosion = 23,
            Outdoors = 24,
            Armor = 26,
            GibLit = 27,
            VirtualRender = 28,
            SandboxGrabba = 29
        }

        public static Config.ConfigJSON Config
            => VRTRAKILL.Config.ConfigJSON.Deserialize();

        public static bool IsMainMenu
            => GameObject.Find("Main Menu State") != null && GameObject.Find("Main Menu State").activeSelf == true;
        public static bool IsPaused
            => OptionsManager.Instance != null && OptionsManager.Instance.paused;
        public static bool IsRankingScreenPresent
            => FinalRank.Instance != null && FinalRank.Instance.isActiveAndEnabled;
        public static bool IsWeaponWheelPresent
            => WeaponWheel.Instance != null && WeaponWheel.Instance.isActiveAndEnabled;
        public static bool IsPlayerUsingShop
            => FistControl.Instance != null && FistControl.Instance.shopping;
        public static bool IsIntro
            => GameObject.Find("Intro") != null && GameObject.Find("Intro").GetComponent<IntroTextController>().enabled;

        public static bool IsSpawnMenuPresent
            => SpawnMenu.Instance != null && SpawnMenu.Instance.isActiveAndEnabled;
        public static bool IsAlterMenuPresent
            => GameObject.Find("Sandbox Alter Menu") != null
            && GameObject.Find("Sandbox Alter Menu").GetComponent<MenuEsc>().isActiveAndEnabled;
        public static bool IsTeleportMenuPresent
            => GameObject.Find("Cheats Teleport") != null
            && GameObject.Find("Cheats Teleport").GetComponent<MenuEsc>().isActiveAndEnabled;

        public static bool IsAMenu
            => IsMainMenu
            || IsPaused
            || IsRankingScreenPresent
            || IsIntro

            || IsSpawnMenuPresent
            || IsAlterMenuPresent
            || IsTeleportMenuPresent;

        public static GameObject VRCameraContainer
            => VRPlayer.VRCamera.Patches.CameraConverterP.Container;
        public static Camera DesktopCamera
            => VRPlayer.VRCamera.Patches.CameraConverterP.DesktopWorldCam;

        public static GameObject LeftController
            => VRPlayer.Controllers.ArmController.Instance.Offset;
        public static VRPlayer.Controllers.ArmController LCC
            => VRPlayer.Controllers.ArmController.Instance;
        public static GameObject RightController
            => VRPlayer.Controllers.GunController.Instance.Offset;
        public static VRPlayer.Controllers.GunController RCC
            => VRPlayer.Controllers.GunController.Instance;

        private static Camera _MainCamera; public static Camera MainCamera
        {
            get { if (_MainCamera == null) { _MainCamera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>(); return _MainCamera; }
                  else return _MainCamera; }
        }
        public static Camera VRUICamera
            => UI.UIConverter.UICamera;
    }
}
