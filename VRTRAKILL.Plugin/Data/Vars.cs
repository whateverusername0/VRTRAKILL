using BepInEx.Logging;
using UnityEngine;
using VRTRAKILL.Prefs;

namespace VRTRAKILL.Data;

public static class Vars
{
    public static VrtrakillConfigJSON Config => ConfigJSON.Instance.Config;
    public static ManualLogSource Log => Plugin.Log;

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
    public static Transform VRCameraContainer
        => Systems.VRCamera.Patches.CameraConverterP.Container.transform;

    public static Transform MainCamera
        => Camera.main.transform;

    public static Camera UICamera
        => Systems.UI.UIConverter.UICamera;

    public static GameObject DesktopCamera
        => Systems.VRCamera.Patches.CameraConverterP.DesktopWorldCam.gameObject;

    public static GameObject DesktopUICamera
        => Systems.VRCamera.Patches.CameraConverterP.DesktopUICam.gameObject;
    #endregion

    #region Controllers
    public static Transform NonDominantHand
        => Systems.Controllers.VRArmsSystem.Instance.GunOffset.transform;
    public static Systems.Controllers.VRArmsSystem NDHC
        => Systems.Controllers.VRArmsSystem.Instance;
    public static Transform DominantHand
        => Systems.Controllers.VRGunsSystem.Instance.GunOffset.transform;
    public static Systems.Controllers.VRGunsSystem DHC
        => Systems.Controllers.VRGunsSystem.Instance;
    #endregion
}