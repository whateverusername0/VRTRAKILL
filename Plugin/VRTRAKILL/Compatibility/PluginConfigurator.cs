using PluginConfig.API;
using PluginConfig.API.Fields;
using PluginConfig.API.Decorators;
using PluginConfig.API.Functionals;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Object = UnityEngine.Object;
using Plugin.Util;
using Plugin.VRTRAKILL.Input;

namespace Plugin.VRTRAKILL.Compatibility
{
    internal static class PluginConfigurator
    {
        // You gotta love pluginconfigurator for all the bloat you need to write :) (eternalUnion i fucking hate you)

        private static PluginConfig.API.PluginConfigurator PC;

        // this shit doesn't scale!
        public static ConfigPanel UKKeybinds;
        static KeyCodeField
            Shoot, AltShoot, Punch,
            Jump, Slide, Dash,
            LastWeaponUsed, ChangeWeaponVariation,
            SwapHand, Whiplash, Escape,
            Slot1, Slot2, Slot3, Slot4, Slot5, Slot6, Slot7, Slot8, Slot9, Slot0;
        static StringField IterateWeapon;

        public static ConfigPanel VRKeybinds;



        public static void Init()
        {
            PC = PluginConfig.API.PluginConfigurator.Create(PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_GUID);
            new ButtonField(PC.rootPanel, "RESTART SCENE (UPDATES CONFIG)", "btnRestartScene").onClick += RestartScene;
            new ConfigHeader(PC.rootPanel, "WORK IN PROGRESS! All of the fields below are WORKING, but the rest is incomplete.") { textColor = Color.red };

            AddUKKeybindsPanel();
            AddVRKeybindsPanel();
        }

        private static void AddUKKeybindsPanel()
        {
            UKKeybinds = new ConfigPanel(PC.rootPanel, "ULTRAKILL Keybinds (MUST BE IN SYNC WITH THE BASE GAME!)", "UKKeybinds");

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

            Shoot.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Shoot), Enum.GetName(typeof(KeyCode), o.value)); };
            AltShoot.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.AltShoot), Enum.GetName(typeof(KeyCode), o.value)); };
            Punch.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Punch), Enum.GetName(typeof(KeyCode), o.value)); };
            Jump.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Jump), Enum.GetName(typeof(KeyCode), o.value)); };
            Slide.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slide), Enum.GetName(typeof(KeyCode), o.value)); };
            Dash.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Dash), Enum.GetName(typeof(KeyCode), o.value)); };
            LastWeaponUsed.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.LastWeaponUsed), Enum.GetName(typeof(KeyCode), o.value)); };
            ChangeWeaponVariation.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.ChangeWeaponVariation), Enum.GetName(typeof(KeyCode), o.value)); };
            IterateWeapon.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.IterateWeapon), Enum.GetName(typeof(KeyCode), o.value)); };
            SwapHand.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.SwapHand), Enum.GetName(typeof(KeyCode), o.value)); };
            Whiplash.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Whiplash), Enum.GetName(typeof(KeyCode), o.value)); };
            Escape.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Escape), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot1.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot1), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot2.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot2), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot3.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot3), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot4.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot4), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot5.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot5), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot6.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot6), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot7.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot7), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot8.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot8), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot9.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot9), Enum.GetName(typeof(KeyCode), o.value)); };
            Slot0.onValueChange += (o) => { Vars.Config.ChangeWrite(nameof(Vars.Config.UKKeybinds.Slot0), Enum.GetName(typeof(KeyCode), o.value)); };
        }
        private static void AddVRKeybindsPanel()
        {
            VRKeybinds = new ConfigPanel(PC.rootPanel, "VRTRAKILL Keybinds", "VRKeybinds");
        }

        public static void RestartScene()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
