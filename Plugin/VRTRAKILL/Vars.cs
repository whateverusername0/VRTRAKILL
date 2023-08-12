using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRAvatar.Armature;

namespace Plugin.VRTRAKILL
{
    public static class Vars
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
        public static Config.NewConfig Config => VRTRAKILL.Config.ConfigJSON.GetConfig().Config;

        #region Checks

        public static bool IsMainMenu
            => OptionsManager.Instance != null && OptionsManager.Instance.mainMenu;
        public static bool IsPaused
            => OptionsManager.Instance != null && OptionsManager.Instance.paused;
        public static bool IsWeaponWheelPresent
            => WeaponWheel.Instance != null && WeaponWheel.Instance.isActiveAndEnabled;
        public static bool IsPlayerUsingShop
            => FistControl.Instance != null && FistControl.Instance.shopping;

        #endregion

        public static bool IsPlayerFrozen
            => (NewMovement.Instance != null && (!NewMovement.Instance.activated || !NewMovement.Instance.enabled))
            || (CameraController.Instance != null && !CameraController.Instance.activated);

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
        public static GameObject SpectatorCamera => VRPlayer.VRCamera.Patches.CameraConverterP.SpectatorCam.transform.parent.gameObject;

        public static GameObject NonDominantHand => VRPlayer.Controllers.ArmController.Instance.GunOffset;
        public static VRPlayer.Controllers.ArmController NDHC => VRPlayer.Controllers.ArmController.Instance;
        public static GameObject DominantHand => VRPlayer.Controllers.GunController.Instance.GunOffset;
        public static VRPlayer.Controllers.GunController DHC => VRPlayer.Controllers.GunController.Instance;
    }
}