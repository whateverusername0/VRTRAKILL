using BepInEx.Logging;
using UnityEngine;
using VRTRAKILL.Prefs;
using VRTRAKILL.Systems;
using VRTRAKILL.Systems.Controllers;

namespace VRTRAKILL.Data;

public static class GlobalVars
{
    public static VrtrakillConfigJSON Config => ConfigJSON.Instance.Config;
    public static ManualLogSource Log => Plugin.Log;

    public static float DefaultRefreshRate = 72f;

    #region Helpers

    public static bool IsMainMenu
        => OptionsManager.Instance?.mainMenu ?? false;

    public static bool IsWeaponWheelPresent
    => WeaponWheel.Instance?.isActiveAndEnabled ?? false;

    public static bool IsPlayerUsingShop
        => FistControl.Instance?.shopping ?? false;

    public static bool IsPlayerFrozen
        => (!NewMovement.Instance?.activated ?? false) || (!NewMovement.Instance?.enabled ?? false)
        || (!CameraController.Instance?.activated ?? false);

    #endregion

    #region Transforms

    public static Transform VRCameraContainer { get; set; }
    public static Transform MainCamera => Camera.main.transform;
    public static Camera UICamera => VRCanvasHelper.UICamera;
    public static GameObject DesktopCamera { get; set; }
    public static GameObject DesktopUICamera { get; set; }

    public static Transform NonDominantHand
        => ArmsVRController.Instance.GunOffset.transform;

    public static ArmsVRController ArmsController
        => ArmsVRController.Instance;

    public static Transform DominantHand
        => GunsVRController.Instance.GunOffset.transform;

    public static GunsVRController GunsController
        => GunsVRController.Instance;

    #endregion
}