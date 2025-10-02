using UnityEngine;

namespace Plugin.Data;

public static class Vars
{
    public static Prefs.NewConfig Config => Prefs.ConfigJSON.GetConfig().Config;
    public static BepInEx.Logging.ManualLogSource Log => Plugin.Log;

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
        => Systems.VRCamera.Patches.CameraConverterP.Container;
    private static Camera _MainCamera; public static Camera MainCamera
    {
        get
        {
            if (_MainCamera == null)
            {
                _MainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.gameObject.GetComponent<Camera>();
                return _MainCamera;
            }
            else return _MainCamera;
        }
    }
    public static Camera UICamera
        => Systems.UI.UIConverter.UICamera;
    public static Camera DesktopCamera
        => Systems.VRCamera.Patches.CameraConverterP.DesktopWorldCam;
    public static Camera DesktopUICamera
        => Systems.VRCamera.Patches.CameraConverterP.DesktopUICam;
    #endregion

    #region Controllers
    public static GameObject NonDominantHand
        => Systems.Controllers.VRArmsSystem.Instance.GunOffset;
    public static Systems.Controllers.VRArmsSystem NDHC
        => Systems.Controllers.VRArmsSystem.Instance;
    public static GameObject DominantHand
        => Systems.Controllers.VRGunsSystem.Instance.GunOffset;
    public static Systems.Controllers.VRGunsSystem DHC
        => Systems.Controllers.VRGunsSystem.Instance;
    #endregion
}