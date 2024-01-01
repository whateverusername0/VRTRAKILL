﻿using UnityEngine;

namespace VRBasePlugin.ULTRAKILL
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
    public static class Vars
    {
        public static Config.NewConfig Config => ULTRAKILL.Config.ConfigJSON.GetConfig().Config;
        public static BepInEx.Logging.ManualLogSource Log => Plugin.PLog;

        #region Checks n' Shits
        public static bool IsMainMenu
            => OptionsManager.Instance?.mainMenu ?? false;
        public static bool IsPaused
            => OptionsManager.Instance?.paused ?? false;
        public static bool IsWeaponWheelPresent
        => WeaponWheel.Instance?.isActiveAndEnabled ?? false;
        public static bool IsPlayerUsingShop
            => FistControl.Instance?.shopping ?? false;

        public static bool IsPlayerFrozen
            => (!NewMovement.Instance?.activated ?? false) || (!NewMovement.Instance?.enabled ?? false)
            || (!CameraController.Instance?.activated ?? false);
        #endregion

        #region Cameras
        public static GameObject VRCameraContainer
            => VRPlayer.VRCamera.Patches.CameraConverterP.Container;
        private static Camera _MainCamera; public static Camera MainCamera
        {
            get
            {
                if (_MainCamera == null)
                {
                    _MainCamera = GameObject.Find("Main Camera")?.gameObject.GetComponent<Camera>();
                    return _MainCamera;
                }
                else return _MainCamera;
            }
        }
        public static Camera UICamera
            => UI.UIConverter.UICamera;
        public static Camera DesktopCamera
            => VRPlayer.VRCamera.Patches.CameraConverterP.DesktopWorldCam;
        public static Camera DesktopUICamera
            => VRPlayer.VRCamera.Patches.CameraConverterP.DesktopUICam;
        #endregion

        #region Controllers
        public static GameObject NonDominantHand
            => VRPlayer.Controllers.ArmController.Instance.GunOffset;
        public static VRPlayer.Controllers.ArmController NDHC
            => VRPlayer.Controllers.ArmController.Instance;
        public static GameObject DominantHand
            => VRPlayer.Controllers.GunController.Instance.GunOffset;
        public static VRPlayer.Controllers.GunController DHC
            => VRPlayer.Controllers.GunController.Instance;
        #endregion
    }
}