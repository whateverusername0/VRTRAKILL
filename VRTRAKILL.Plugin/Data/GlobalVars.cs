namespace VRTRAKILL.Plugin.Data;

public static class GlobalVars
{
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
}