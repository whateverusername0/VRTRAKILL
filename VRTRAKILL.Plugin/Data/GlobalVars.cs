using BepInEx.Logging;
using UnityEngine;
using VRTRAKILL.Prefs;

namespace VRTRAKILL.Data;

public static class GlobalVars
{
    public static VrtrakillConfigJSON Config => ConfigJSON.Instance.Config;
    public static ManualLogSource Log => Plugin.Log;

    public static float RefreshRate = 72f;

    // checks
    public static bool IsMainMenu
        => OptionsManager.Instance?.mainMenu ?? false;
    public static bool IsWeaponWheelPresent
    => WeaponWheel.Instance?.isActiveAndEnabled ?? false;
    public static bool IsPlayerUsingShop
        => FistControl.Instance?.shopping ?? false;
    public static bool IsPlayerFrozen
        => (!NewMovement.Instance?.activated ?? false) || (!NewMovement.Instance?.enabled ?? false)
        || (!CameraController.Instance?.activated ?? false);

    // cameras
    public static Transform VRCameraContainer { get; set; }
    public static Transform MainCamera => Camera.main.transform;
    public static Camera UICamera => Systems.UI.VRUIConverter.UICamera;
    public static GameObject DesktopCamera { get; set; }
    public static GameObject DesktopUICamera { get; set; }

    // controllers
    public static Transform NonDominantHand
        => Systems.Controllers.VRArmsSystem.Instance.GunOffset.transform;
    public static Systems.Controllers.VRArmsSystem NDHC
        => Systems.Controllers.VRArmsSystem.Instance;
    public static Transform DominantHand
        => Systems.Controllers.VRGunsSystem.Instance.GunOffset.transform;
    public static Systems.Controllers.VRGunsSystem DHC
        => Systems.Controllers.VRGunsSystem.Instance;
}