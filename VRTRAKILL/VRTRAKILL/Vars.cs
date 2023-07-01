using UnityEngine;

namespace Plugin.VRTRAKILL
{
    static class Vars
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

        public static Config.ConfigJSON Config => VRTRAKILL.Config.ConfigJSON.GetConfig();

        #region Menu detector stuff

        public static bool IsMainMenu
            => GameObject.Find("Main Menu State") != null
            && GameObject.Find("Main Menu State") != null
            && GameObject.Find("Main Menu State").activeSelf == true;
        public static bool IsPaused
            => OptionsManager.Instance != null && OptionsManager.Instance.paused;
        public static bool IsRankingScreenPresent
            => FinalRank.Instance != null && FinalRank.Instance.isActiveAndEnabled;
        public static bool IsWeaponWheelPresent
            => WeaponWheel.Instance != null && WeaponWheel.Instance.isActiveAndEnabled;
        public static bool IsPlayerUsingShop
            => FistControl.Instance != null && FistControl.Instance.shopping;
        public static bool IsIntro
            => GameObject.Find("Intro") != null
            && GameObject.Find("Intro").GetComponent<IntroTextController>() != null
            && GameObject.Find("Intro").GetComponent<IntroTextController>().enabled;
        public static bool IsActEndPresent
            => GameObject.Find("Act End Message") != null
            && GameObject.Find("Act End Message").activeInHierarchy;

        public static bool IsSpawnMenuPresent
            => SpawnMenu.Instance != null && SpawnMenu.Instance.isActiveAndEnabled;
        public static bool IsAlterMenuPresent
            => GameObject.Find("Sandbox Alter Menu") != null
            && GameObject.Find("Sandbox Alter Menu").GetComponent<MenuEsc>().isActiveAndEnabled;
        public static bool IsTeleportMenuPresent
            => GameObject.Find("Cheats Teleport") != null
            && GameObject.Find("Cheats Teleport").GetComponent<MenuEsc>().isActiveAndEnabled;

        #endregion

        public static bool IsAMenu
            => IsMainMenu
            || IsPaused
            || IsRankingScreenPresent
            || IsIntro

            || IsSpawnMenuPresent
            || IsAlterMenuPresent
            || IsTeleportMenuPresent
            || IsActEndPresent;

        public static GameObject VRCameraContainer => VRPlayer.VRCamera.Patches.CameraConverterP.Container;
        private static Camera _MainCamera; public static Camera MainCamera
        {
            get
            {
                if (_MainCamera == null) { _MainCamera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>(); return _MainCamera; }
                else return _MainCamera;
            }
        }
        public static Camera UICamera => UI.UIConverter.UICamera;
        public static Camera DesktopCamera => VRPlayer.VRCamera.Patches.CameraConverterP.DesktopWorldCam;
        public static Camera DesktopUICamera => VRPlayer.VRCamera.Patches.CameraConverterP.DesktopUICam;

        public static GameObject NonDominantHand => VRPlayer.Controllers.ArmController.Instance.Offset;
        public static VRPlayer.Controllers.ArmController NDHC => VRPlayer.Controllers.ArmController.Instance;
        public static GameObject DominantHand => VRPlayer.Controllers.GunController.Instance.Offset;
        public static VRPlayer.Controllers.GunController DHC => VRPlayer.Controllers.GunController.Instance;

        #region Arms

        public static VRPlayer.VRIK.Armature FeedbackerArm { get; set; }
        public static VRPlayer.VRIK.Armature KnuckleblasterArm { get; set; }
        public static VRPlayer.VRIK.Armature SpearArm { get; set; }
        public static VRPlayer.VRIK.Armature WhiplashArm { get; set; }
        public static VRPlayer.VRIK.Armature SandboxerArm { get; set; }

        #endregion

        #region Player skins

        public static VRPlayer.VRIK.MetaRig V1Rig { get; set; }
        public static VRPlayer.VRIK.MetaRig V2Rig { get; set; }

        #endregion
    }
}
