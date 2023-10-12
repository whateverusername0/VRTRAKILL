using PluginConfig.API;
using PluginConfig.API.Fields;
using PluginConfig.API.Decorators;
using PluginConfig.API.Functionals;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Object = UnityEngine.Object;
using Plugin.Util;

namespace Plugin.VRTRAKILL.Compatibility
{
    internal static class PluginConfigurator
    {
        // You gotta love pluginconfigurator for all the bloat you need to write :) (eternalUnion i fucking hate you)

        private static PluginConfig.API.PluginConfigurator PC;

        // this shit doesn't scale!
        #region UKKeybinds
        public static ConfigPanel UKKeybinds;
        static KeyCodeField
            Shoot, AltShoot, Punch,
            Jump, Slide, Dash,
            LastWeaponUsed, ChangeWeaponVariation,
            SwapHand, Whiplash, Escape,
            Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8, Slot9, Slot0;
        static StringField IterateWeapon;
        #endregion
        #region VRKeybinds
        public static ConfigPanel VRKeybinds;
        static KeyCodeField
            ToggleDV, ToggleTPC, EnumTPCMode,
            TPCamUp, TPCamDown, TPCamLeft, TPCamRight,
            TPCamMoveMode,
            ToggleAvatarCalibration;
        #endregion
        static FloatField MovementMultiplier;
        #region Controllers
        static ConfigPanel Controllers;
        static FloatField Deadzone, SmoothSpeed, SnapAngles;
        static BoolField SnapTurn, DrawControllers, EnableHaptics, LeftHanded;
        #endregion
        #region CBS
        static ConfigPanel CBS;
        static BoolField EnableCBS;
        static BoolField EnableCrosshair;
        static FloatField CrosshairDistance;
        #endregion
        #region MBP
        static ConfigPanel MBP;
        static BoolField EnableMBP;
        static BoolField ToggleVelocity, EnableNDHCoinThrow, CameraWhiplash;
        static FloatField PunchingSpeed;
        #endregion
        #region VRBody
        static ConfigPanel VRBody;
        static BoolField EnableVRBody;
        #endregion
        #region UIInteraction
        static ConfigPanel UIInteraction;
        static FloatField UISize;
        static BoolField ControllerBased;
        static BoolField CLEnabled; static FloatField StartAlpha, EndAlpha;
        #endregion
        #region DesktopView
        static ConfigPanel DesktopView;
        static BoolField DVEnabled;
        static FloatField WCFOV, UICFOV;

        static BoolField SCEnabled;
        static FloatField SCMode;
        #endregion
        #region Misc
        static ConfigPanel Misc;
        #endregion

        public static void Init()
        {
            PC = PluginConfig.API.PluginConfigurator.Create(PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_GUID);
            PC.SetIconWithURL($"file://{PluginInfo.PluginPath}\\icon.png");
            PC.saveToFile = false;
            new ButtonField(PC.rootPanel, "RESET (RESTARTS THE GAME)", "btnRestartScene").onClick += RESET;
            new ConfigHeader(PC.rootPanel, "To apply settings, RESET by pressing the button above.") { textColor = Color.yellow };

            AddUKKeybindsPanel();
            AddVRKeybindsPanel();

            MovementMultiplier = new FloatField(PC.rootPanel, "Movement Multiplier (affects player force)", "MMP", Vars.Config.MovementMultiplier);
            MovementMultiplier.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.MovementMultiplier), o.value);

            AddControllersPanel();
            AddCBSPanel();
            AddMBPPanel();
            AddVRBodyPanel();
            AddUIIPanel();
            AddDVPanel();
            AddMiscPanel();
        }

        private static void AddUKKeybindsPanel()
        {
            UKKeybinds = new ConfigPanel(PC.rootPanel, "ULTRAKILL Keybinds", "UKKeybinds");
            new ConfigHeader(UKKeybinds, "MUST BE IN SYNC WITH THE BASE GAME!") { textColor = Color.yellow };

            Shoot = new KeyCodeField(UKKeybinds, "Shoot", "ukkShoot", Vars.Config.UKKeybinds.Shoot.ToKeyCode());
            AltShoot = new KeyCodeField(UKKeybinds, "Alternative Fire", "ukkAltShoot", Vars.Config.UKKeybinds.AltShoot.ToKeyCode());
            Punch = new KeyCodeField(UKKeybinds, "Punch", "ukkPunch", Vars.Config.UKKeybinds.Punch.ToKeyCode());
            Jump = new KeyCodeField(UKKeybinds, "Jump", "ukkJump", Vars.Config.UKKeybinds.Jump.ToKeyCode());
            Slide = new KeyCodeField(UKKeybinds, "Slide", "ukkSlide", Vars.Config.UKKeybinds.Slide.ToKeyCode());
            Dash = new KeyCodeField(UKKeybinds, "Dash", "ukkDash", Vars.Config.UKKeybinds.Dash.ToKeyCode());
            LastWeaponUsed = new KeyCodeField(UKKeybinds, "Last Weapon Used", "ukkLWU", Vars.Config.UKKeybinds.LastWeaponUsed.ToKeyCode());
            ChangeWeaponVariation = new KeyCodeField(UKKeybinds, "Change Weapon Variation", "ukkCWV", Vars.Config.UKKeybinds.ChangeWeaponVariation.ToKeyCode());
            IterateWeapon = new StringField(UKKeybinds, "Iterate Weapon (CHANGE DIRECTLY IN FILE)", "ukkIterateWeapon", Vars.Config.UKKeybinds.IterateWeapon) { interactable = false };
            SwapHand = new KeyCodeField(UKKeybinds, "Swap Hand", "ukkSwapHand", Vars.Config.UKKeybinds.SwapHand.ToKeyCode());
            Whiplash = new KeyCodeField(UKKeybinds, "Whiplash", "ukkWhiplash", Vars.Config.UKKeybinds.Whiplash.ToKeyCode());
            Escape = new KeyCodeField(UKKeybinds, "Escape", "ukkEscape", Vars.Config.UKKeybinds.Escape.ToKeyCode());
            Slot1 = new KeyCodeField(UKKeybinds, "Slot 1", "ukkSlot1", Vars.Config.UKKeybinds.Slot1.ToKeyCode());
            Slot2 = new KeyCodeField(UKKeybinds, "Slot 2", "ukkSlot2", Vars.Config.UKKeybinds.Slot2.ToKeyCode());
            Slot3 = new KeyCodeField(UKKeybinds, "Slot 3", "ukkSlot3", Vars.Config.UKKeybinds.Slot3.ToKeyCode());
            Slot4 = new KeyCodeField(UKKeybinds, "Slot 4", "ukkSlot4", Vars.Config.UKKeybinds.Slot4.ToKeyCode());
            Slot5 = new KeyCodeField(UKKeybinds, "Slot 5", "ukkSlot5", Vars.Config.UKKeybinds.Slot5.ToKeyCode());
            Slot6 = new KeyCodeField(UKKeybinds, "Slot 6", "ukkSlot6", Vars.Config.UKKeybinds.Slot6.ToKeyCode());
            Slot7 = new KeyCodeField(UKKeybinds, "Slot 7", "ukkSlot7", Vars.Config.UKKeybinds.Slot7.ToKeyCode());
            Slot8 = new KeyCodeField(UKKeybinds, "Slot 8", "ukkSlot8", Vars.Config.UKKeybinds.Slot8.ToKeyCode());
            Slot9 = new KeyCodeField(UKKeybinds, "Slot 9", "ukkSlot9", Vars.Config.UKKeybinds.Slot9.ToKeyCode());
            Slot0 = new KeyCodeField(UKKeybinds, "Slot 0", "ukkSlot0", Vars.Config.UKKeybinds.Slot0.ToKeyCode());

            Shoot.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Shoot), o.value);
            AltShoot.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.AltShoot), o.value);
            Punch.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Punch), o.value);
            Jump.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Jump), o.value);
            Slide.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slide), o.value);
            Dash.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Dash), o.value);
            LastWeaponUsed.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.LastWeaponUsed), o.value);
            ChangeWeaponVariation.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.ChangeWeaponVariation), o.value);
            IterateWeapon.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.IterateWeapon), o.value);
            SwapHand.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.SwapHand), o.value);
            Whiplash.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Whiplash), o.value);
            Escape.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Escape), o.value);
            Slot1.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot1), o.value);
            Slot2.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot2), o.value);
            Slot3.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot3), o.value);
            Slot4.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot4), o.value);
            Slot5.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot5), o.value);
            Slot6.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot6), o.value);
            Slot7.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot7), o.value);
            Slot8.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot8), o.value);
            Slot9.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot9), o.value);
            Slot0.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.UKKeybinds.Slot0), o.value);
        }
        private static void AddVRKeybindsPanel()
        {
            VRKeybinds = new ConfigPanel(PC.rootPanel, "VRTRAKILL Keybinds", "VRKeybinds");
            ToggleDV = new KeyCodeField(VRKeybinds, "Toggle DesktopView", "ToggleDV", Vars.Config.VRKeybinds.ToggleDV.ToKeyCode());
            ToggleTPC = new KeyCodeField(VRKeybinds, "Toggle ThirdPersonCamera", "ToggleTPC", Vars.Config.VRKeybinds.ToggleTPC.ToKeyCode());
            EnumTPCMode = new KeyCodeField(VRKeybinds, "Switch TPC mode", "EnumTPCMode", Vars.Config.VRKeybinds.EnumTPCMode.ToKeyCode());
            TPCamUp = new KeyCodeField(VRKeybinds, "TPCam Up", "TPCUp", Vars.Config.VRKeybinds.TPCamUp.ToKeyCode());
            TPCamDown = new KeyCodeField(VRKeybinds, "TPCam Down", "TPCDown", Vars.Config.VRKeybinds.TPCamDown.ToKeyCode());
            TPCamLeft = new KeyCodeField(VRKeybinds, "TPCam Left", "TPCLeft", Vars.Config.VRKeybinds.TPCamLeft.ToKeyCode());
            TPCamRight = new KeyCodeField(VRKeybinds, "TPCam Right", "TPCRight", Vars.Config.VRKeybinds.TPCamRight.ToKeyCode());
            TPCamMoveMode = new KeyCodeField(VRKeybinds, "TPCam move mode", "", Vars.Config.VRKeybinds.TPCamMoveMode.ToKeyCode());
            ToggleAvatarCalibration = new KeyCodeField(VRKeybinds, "Toggle Avatar Calibration (PLACEHOLDER)", "ToggleAvatarC", Vars.Config.VRKeybinds.ToggleAvatarCalibration.ToKeyCode());

            ToggleDV.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.ToggleDV), o.value);
            ToggleTPC.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.ToggleTPC), o.value);
            EnumTPCMode.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.EnumTPCMode), o.value);
            TPCamUp.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.TPCamUp), o.value);
            TPCamDown.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.TPCamDown), o.value);
            TPCamLeft.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.TPCamLeft), o.value);
            TPCamRight.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.TPCamRight), o.value);
            TPCamMoveMode.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.TPCamMoveMode), o.value);
            ToggleAvatarCalibration.onValueChange += (o) => ChangeKeyCode(nameof(Vars.Config.VRKeybinds.ToggleAvatarCalibration), o.value);
        }
        private static void AddControllersPanel()
        {
            Controllers = new ConfigPanel(PC.rootPanel, "Controllers Settings", "Controllers");
            Deadzone = new FloatField(Controllers, "Deadzone", "Deadzone", Vars.Config.Controllers.Deadzone);
            SmoothSpeed = new FloatField(Controllers, "Smooth turning speed", "SmoothS", Vars.Config.Controllers.SmoothSpeed);
            SnapTurn = new BoolField(Controllers, "Snap turning (overrides smooth turning)", "SnapTurn", Vars.Config.Controllers.SnapTurn);
            SnapAngles = new FloatField(Controllers, "Snap turning angles", "SnapA", Vars.Config.Controllers.SnapAngles);
            DrawControllers = new BoolField(Controllers, "Draw controller models", "DrawControllers", Vars.Config.Controllers.DrawControllers);
            EnableHaptics = new BoolField(Controllers, "Enable controller rumble", "EnableHaptics", Vars.Config.Controllers.EnableHaptics);
            LeftHanded = new BoolField(Controllers, "Left-handed mode (BROKEN)", "LHM", Vars.Config.Controllers.LeftHanded);

            Deadzone.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.Controllers.Deadzone), o.value);
            SmoothSpeed.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.Controllers.SmoothSpeed), o.value);
            SnapTurn.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.Controllers.SnapTurn), o.value);
            SnapAngles.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.Controllers.SnapAngles), o.value);
            DrawControllers.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.Controllers.DrawControllers), o.value);
            EnableHaptics.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.Controllers.EnableHaptics), o.value);
            LeftHanded.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.Controllers.LeftHanded), o.value);
        }
        private static void AddCBSPanel()
        {
            CBS = new ConfigPanel(PC.rootPanel, "Controller-based Shooting", "CBS");
            EnableCBS = new BoolField(CBS, "Enabled", "ECBS", Vars.Config.EnableCBS);
            ConfigDivision CBSCD = new ConfigDivision(CBS, "CBSCD");
            EnableCrosshair = new BoolField(CBSCD, "Enable Crosshair", "EnableCrosshair", Vars.Config.CBS.EnableCrosshair);
            CrosshairDistance = new FloatField(CBSCD, "Crosshair distance (from the hand)", "CrosshairDistance", Vars.Config.CBS.CrosshairDistance);

            EnableCBS.onValueChange += (o) => { CBSCD.interactable = o.value; Vars.Config.ChangeWrite(nameof(Vars.Config.EnableCBS), o.value); };
            EnableCrosshair.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.CBS.EnableCrosshair), o.value);
            CrosshairDistance.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.CBS.CrosshairDistance), o.value);
        }
        private static void AddMBPPanel()
        {
            MBP = new ConfigPanel(PC.rootPanel, "Movement-based Punching", "MBP");
            EnableMBP = new BoolField(MBP, "Enabled", "EMBP", Vars.Config.EnableMBP);
            ConfigDivision MBPCD = new ConfigDivision(MBP, "MBPCD");
            ToggleVelocity = new BoolField(MBP, "Velocity-based punching direction", "ToggleVelocity", Vars.Config.MBP.ToggleVelocity);
            PunchingSpeed = new FloatField(MBP, "Required speed to punch", "PunchingSpeed", Vars.Config.MBP.PunchingSpeed);
            EnableNDHCoinThrow = new BoolField(MBP, "Enable coin throwing from the non-dominant hand", "EnableNDHCT", Vars.Config.MBP.EnableNDHCoinThrow);
            CameraWhiplash = new BoolField(MBP, "WHIPLASH: camera-based aiming", "CameraWhiplash", Vars.Config.MBP.CameraWhiplash);

            EnableMBP.onValueChange += (o) => { MBPCD.interactable = o.value; Vars.Config.ChangeWrite(nameof(Vars.Config.EnableMBP), o.value); };
            ToggleVelocity.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.MBP.ToggleVelocity), o.value);
            PunchingSpeed.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.MBP.PunchingSpeed), o.value);
            EnableNDHCoinThrow.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.MBP.EnableNDHCoinThrow), o.value);
            CameraWhiplash.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.MBP.CameraWhiplash), o.value);
        }
        private static void AddVRBodyPanel()
        {
            VRBody = new ConfigPanel(PC.rootPanel, "VR Avatar", "VRBody");
            ConfigDivision VRBCD = new ConfigDivision(VRBody, "VRBCD");
            EnableVRBody = new BoolField(VRBody, "Enabled", "EnableVRB", Vars.Config.EnableVRBody);

            EnableVRBody.onValueChange += (o) => { VRBCD.interactable = o.value; Vars.Config.ChangeWrite(nameof(Vars.Config.EnableVRBody), o.value); };
        }
        private static void AddUIIPanel()
        {
            UIInteraction = new ConfigPanel(PC.rootPanel, "UI Interaction", "UII");
            UISize = new FloatField(UIInteraction, "UI Size (0 - 0.1)", "UISize", Vars.Config.UIInteraction.UISize);
            ControllerBased = new BoolField(UIInteraction, "Controller-based", "ControllerBased", Vars.Config.UIInteraction.ControllerBased);

            new ConfigHeader(UIInteraction, "--- Controller lines ---");
            ConfigDivision CLCD = new ConfigDivision(UIInteraction, "CLCD");
            CLEnabled = new BoolField(UIInteraction, "Enabled", "CLEnabled", Vars.Config.UIInteraction.ControllerLines.Enabled);
            StartAlpha = new FloatField(CLCD, "Initial alpha (0 - 1)", "StartAlpha", Vars.Config.UIInteraction.ControllerLines.StartAlpha);
            EndAlpha = new FloatField(CLCD, "End alpha (0 - 1)", "EndAlpha", Vars.Config.UIInteraction.ControllerLines.EndAlpha);

            UISize.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.UIInteraction.UISize), o.value);
            CLEnabled.onValueChange += (o) => { CLCD.interactable = o.value; Vars.Config.ChangeWrite(nameof(Vars.Config.UIInteraction.ControllerLines.Enabled), o.value); };
            StartAlpha.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.UIInteraction.ControllerLines.StartAlpha), o.value);
            EndAlpha.onValueChange += (o) => Vars.Config.ChangeWrite(nameof(Vars.Config.UIInteraction.ControllerLines.EndAlpha), o.value);
        }
        private static void AddDVPanel()
        {

        }
        private static void AddMiscPanel()
        {
            Misc = new ConfigPanel(PC.rootPanel, "Miscellaneous", "Misc");
            new ConfigHeader(Misc, "There are currently no miscellaenous settings.");
        }


        private static void ChangeKeyCode(string Name, object Value)
        => Vars.Config.ChangeWrite(Name, Enum.GetName(typeof(KeyCode), Value));

        public static void RESET()
        => SceneManager.LoadScene("Bootstrap");
    }
}