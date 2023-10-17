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
        static BoolField EnableCBS;
        #region CBS
        static ConfigPanel CBS;
        static BoolField EnableCrosshair;
        static FloatField CrosshairDistance;
        #endregion
        static BoolField EnableMBP;
        #region MBP
        static ConfigPanel MBP;
        static BoolField ToggleVelocity, CameraWhiplash;
        static FloatField PunchingSpeed;
        #endregion
        static BoolField EnableVRBody;
        #region VRBody
        static ConfigPanel VRBody;
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
            MovementMultiplier.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.MovementMultiplier = v);

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

            Shoot.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Shoot = Enum.GetName(typeof(KeyCode), v));
            AltShoot.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.AltShoot = Enum.GetName(typeof(KeyCode), v));
            Punch.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Punch = Enum.GetName(typeof(KeyCode), v));
            Jump.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Jump = Enum.GetName(typeof(KeyCode), v));
            Slide.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slide = Enum.GetName(typeof(KeyCode), v));
            Dash.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Dash = Enum.GetName(typeof(KeyCode), v));
            LastWeaponUsed.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.LastWeaponUsed = Enum.GetName(typeof(KeyCode), v));
            ChangeWeaponVariation.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.ChangeWeaponVariation = Enum.GetName(typeof(KeyCode), v));
            IterateWeapon.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.IterateWeapon = Enum.GetName(typeof(KeyCode), v));
            SwapHand.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.SwapHand = Enum.GetName(typeof(KeyCode), v));
            Whiplash.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Whiplash = Enum.GetName(typeof(KeyCode), v));
            Escape.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Escape = Enum.GetName(typeof(KeyCode), v));
            Slot1.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot1 = Enum.GetName(typeof(KeyCode), v));
            Slot2.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot2 = Enum.GetName(typeof(KeyCode), v));
            Slot3.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot3 = Enum.GetName(typeof(KeyCode), v));
            Slot4.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot4 = Enum.GetName(typeof(KeyCode), v));
            Slot5.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot5 = Enum.GetName(typeof(KeyCode), v));
            Slot6.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot6 = Enum.GetName(typeof(KeyCode), v));
            Slot7.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot7 = Enum.GetName(typeof(KeyCode), v));
            Slot8.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot8 = Enum.GetName(typeof(KeyCode), v));
            Slot9.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot9 = Enum.GetName(typeof(KeyCode), v));
            Slot0.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UKKeybinds.Slot0 = Enum.GetName(typeof(KeyCode), v));
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

            new ConfigHeader(VRKeybinds, "--- PLACEHOLDER ---");
            ToggleAvatarCalibration = new KeyCodeField(VRKeybinds, "Toggle Avatar Calibration", "ToggleAvatarC", Vars.Config.VRKeybinds.ToggleAvatarCalibration.ToKeyCode());

            ToggleDV.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.ToggleDV = Enum.GetName(typeof(KeyCode), v));
            ToggleTPC.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.ToggleTPC = Enum.GetName(typeof(KeyCode), v));
            EnumTPCMode.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.EnumTPCMode = Enum.GetName(typeof(KeyCode), v));
            TPCamUp.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.TPCamUp = Enum.GetName(typeof(KeyCode), v));
            TPCamDown.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.TPCamDown = Enum.GetName(typeof(KeyCode), v));
            TPCamLeft.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.TPCamLeft = Enum.GetName(typeof(KeyCode), v));
            TPCamRight.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.TPCamRight = Enum.GetName(typeof(KeyCode), v));
            TPCamMoveMode.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.TPCamMoveMode = Enum.GetName(typeof(KeyCode), v));
            ToggleAvatarCalibration.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.VRKeybinds.ToggleAvatarCalibration = Enum.GetName(typeof(KeyCode), v));
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

            new ConfigHeader(Controllers, "--- BROKEN ---") { textColor = Color.red };
            LeftHanded = new BoolField(Controllers, "Left-handed mode (BROKEN)", "LHM", Vars.Config.Controllers.LeftHanded);

            Deadzone.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.Controllers.Deadzone = v);
            SmoothSpeed.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.Controllers.SmoothSpeed = v);
            SnapTurn.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.Controllers.SnapTurn = v);
            SnapAngles.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.Controllers.SnapAngles = v);
            DrawControllers.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.Controllers.DrawControllers = v);
            EnableHaptics.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.Controllers.EnableHaptics = v);
            LeftHanded.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.Controllers.LeftHanded = v);
        }
        private static void AddCBSPanel()
        {
            CBS = new ConfigPanel(PC.rootPanel, "Controller-based Shooting", "CBS");
            EnableCBS = new BoolField(CBS, "Enabled", "ECBS", Vars.Config.EnableCBS);
            ConfigDivision CBSCD = new ConfigDivision(CBS, "CBSCD");
            EnableCrosshair = new BoolField(CBSCD, "Enable Crosshair", "EnableCrosshair", Vars.Config.CBS.EnableCrosshair);
            CrosshairDistance = new FloatField(CBSCD, "Crosshair distance (from the hand)", "CrosshairDistance", Vars.Config.CBS.CrosshairDistance);

            EnableCBS.onValueChange += (o) => { CBSCD.interactable = o.value; Vars.Config.ChangeWrite(o.value, v => Vars.Config.EnableCBS = v); };
            EnableCrosshair.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.CBS.EnableCrosshair = v);
            CrosshairDistance.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.CBS.CrosshairDistance = v);
        }
        private static void AddMBPPanel()
        {
            MBP = new ConfigPanel(PC.rootPanel, "Movement-based Punching", "MBP");
            EnableMBP = new BoolField(MBP, "Enabled", "EMBP", Vars.Config.EnableMBP);
            ConfigDivision MBPCD = new ConfigDivision(MBP, "MBPCD");
            ToggleVelocity = new BoolField(MBP, "Velocity-based punching direction", "ToggleVelocity", Vars.Config.MBP.ToggleVelocity);
            PunchingSpeed = new FloatField(MBP, "Required speed to punch", "PunchingSpeed", Vars.Config.MBP.PunchingSpeed);
            CameraWhiplash = new BoolField(MBP, "WHIPLASH: camera-based aiming", "CameraWhiplash", Vars.Config.MBP.CameraWhiplash);

            EnableMBP.onValueChange += (o) => { MBPCD.interactable = o.value; Vars.Config.ChangeWrite(o.value, v => Vars.Config.EnableMBP = v); };
            ToggleVelocity.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.MBP.ToggleVelocity = v);
            PunchingSpeed.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.MBP.PunchingSpeed = v);
            CameraWhiplash.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.MBP.CameraWhiplash = v);
        }
        private static void AddVRBodyPanel()
        {
            VRBody = new ConfigPanel(PC.rootPanel, "VR Avatar", "VRBody");
            ConfigDivision VRBCD = new ConfigDivision(VRBody, "VRBCD");
            EnableVRBody = new BoolField(VRBody, "Enabled", "EnableVRB", Vars.Config.EnableVRBody);
            new ConfigHeader(VRBody, "--- SKINS COMING SOON (not very) ---");

            EnableVRBody.onValueChange += (o) => { VRBCD.interactable = o.value; Vars.Config.ChangeWrite(o.value, v => Vars.Config.EnableVRBody = v); };
        }
        private static void AddUIIPanel()
        {
            UIInteraction = new ConfigPanel(PC.rootPanel, "UI Interaction", "UII");
            UISize = new FloatField(UIInteraction, "UI Size (0 - 0.1)", "UISize", Vars.Config.UIInteraction.UISize);
            ControllerBased = new BoolField(UIInteraction, "Controller-based", "ControllerBased", Vars.Config.UIInteraction.ControllerBased);

            new ConfigHeader(UIInteraction, "--- CONTROLLER LINES ---");
            ConfigDivision CLCD = new ConfigDivision(UIInteraction, "CLCD");
            CLEnabled = new BoolField(UIInteraction, "Enabled", "CLEnabled", Vars.Config.UIInteraction.ControllerLines.Enabled);
            StartAlpha = new FloatField(CLCD, "Initial alpha (0 - 1)", "StartAlpha", Vars.Config.UIInteraction.ControllerLines.StartAlpha);
            EndAlpha = new FloatField(CLCD, "End alpha (0 - 1)", "EndAlpha", Vars.Config.UIInteraction.ControllerLines.EndAlpha);

            UISize.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UIInteraction.UISize = v);
            CLEnabled.onValueChange += (o) => { CLCD.interactable = o.value; Vars.Config.ChangeWrite(o.value, v => Vars.Config.UIInteraction.ControllerLines.Enabled = v); };
            ControllerBased.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UIInteraction.ControllerBased = v);
            StartAlpha.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UIInteraction.ControllerLines.StartAlpha = v);
            EndAlpha.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.UIInteraction.ControllerLines.EndAlpha = v);
        }
        private static void AddDVPanel()
        {
            DesktopView = new ConfigPanel(PC.rootPanel, "Desktop View", "DesktopView");
            ConfigDivision DVCD = new ConfigDivision(DesktopView, "DVCD");
            DVEnabled = new BoolField(DesktopView, "Enabled", "DVEnabled", Vars.Config.DesktopView.Enabled);
            WCFOV = new FloatField(DVCD, "World view FOV", "WCFOV", Vars.Config.DesktopView.WorldCamFOV);
            UICFOV = new FloatField(DVCD, "UI view FOV", "UICFOV", Vars.Config.DesktopView.UICamFOV);

            new ConfigHeader(DesktopView, "--- THIRD PERSON CAMERA ---");
            ConfigDivision SCCD = new ConfigDivision(DesktopView, "SCCD");
            SCEnabled = new BoolField(DesktopView, "Enabled", "SCEnabled", Vars.Config.DesktopView.ThirdPersonCamera.Enabled);
            new ConfigHeader(SCCD, "Modes: 0 - follow, 1 - rotate around, 2 - fixed");
            SCMode = new FloatField(SCCD, "Mode", "SCMode", Vars.Config.DesktopView.ThirdPersonCamera.Mode);

            DVEnabled.onValueChange += (o) => { DVCD.interactable = o.value; Vars.Config.ChangeWrite(o.value, v => Vars.Config.DesktopView.Enabled = v); };
            WCFOV.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.DesktopView.WorldCamFOV = v);
            UICFOV.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.DesktopView.UICamFOV = v);
            SCEnabled.onValueChange += (o) => { SCCD.interactable = o.value; Vars.Config.ChangeWrite(o.value, v => Vars.Config.DesktopView.ThirdPersonCamera.Enabled = v); };
            SCMode.onValueChange += (o) => Vars.Config.ChangeWrite(o.value, v => Vars.Config.DesktopView.ThirdPersonCamera.Mode = (int)v);
        }
        private static void AddMiscPanel()
        {
            Misc = new ConfigPanel(PC.rootPanel, "Miscellaneous", "Misc");
            new ConfigHeader(Misc, "There are currently no miscellaenous settings.");
        }

        public static void RESET() => SceneManager.LoadScene("Bootstrap");
    }
}