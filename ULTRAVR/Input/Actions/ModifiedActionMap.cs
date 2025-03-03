using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace ULTRAVR.Input.Actions
{
    /// <summary>
    ///     Represents the modifiedActions object in Binds.json
    /// </summary>
    public class ModifiedActionMap
    {
        [JsonProperty("Dodge")] public ModifiedAction[] Dodge { get; set; }
        [JsonProperty("Slide")] public ModifiedAction[] Slide { get; set; }
        [JsonProperty("Jump")] public ModifiedAction[] Jump { get; set; }
        [JsonProperty("Change Fist")] public ModifiedAction[] ChangeFist { get; set; }
        [JsonProperty("Punch")] public ModifiedAction[] Punch { get; set; }
        [JsonProperty("Hook")] public ModifiedAction[] Hook { get; set; }
        [JsonProperty("Primary Fire")] public ModifiedAction[] PrimaryFire { get; set; }
        [JsonProperty("Secondary Fire")] public ModifiedAction[] SecondaryFire { get; set; }
        [JsonProperty("Change Variation")] public ModifiedAction[] ChangeVariation { get; set; }
        [JsonProperty("Slot 0")] public ModifiedAction[] Slot0 { get; set; }
        [JsonProperty("Slot 1")] public ModifiedAction[] Slot1 { get; set; }
        [JsonProperty("Slot 2")] public ModifiedAction[] Slot2 { get; set; }
        [JsonProperty("Slot 3")] public ModifiedAction[] Slot3 { get; set; }
        [JsonProperty("Slot 4")] public ModifiedAction[] Slot4 { get; set; }
        [JsonProperty("Slot 5")] public ModifiedAction[] Slot5 { get; set; }
        [JsonProperty("Slot 6")] public ModifiedAction[] Slot6 { get; set; }
        [JsonProperty("Slot 7")] public ModifiedAction[] Slot7 { get; set; }
        [JsonProperty("Slot 8")] public ModifiedAction[] Slot8 { get; set; }
        [JsonProperty("Slot 9")] public ModifiedAction[] Slot9 { get; set; }
        [JsonProperty("Next Weapon")] public ModifiedAction[] NextWeapon { get; set; }
        [JsonProperty("Previous Weapon")] public ModifiedAction[] PrevWeapon { get; set; }
        [JsonProperty("Last Weapon")] public ModifiedAction[] LastWeapon { get; set; }

        /// <summary>
        /// Creates a new <see cref="ModifiedActionMap"/> class with the default game parameters.
        /// </summary>
        public ModifiedActionMap()
        {
            Dodge = new ModifiedAction[] { new ModifiedAction("LeftShift") };
            Slide = new ModifiedAction[] { new ModifiedAction("LeftControl") };
            Jump = new ModifiedAction[] { new ModifiedAction("Space") };
            ChangeFist = new ModifiedAction[] { new ModifiedAction("G") };
            Punch = new ModifiedAction[] { new ModifiedAction("F") };
            Hook = new ModifiedAction[] { new ModifiedAction("R") };
            PrimaryFire = new ModifiedAction[] { new ModifiedAction("LMB") };
            SecondaryFire = new ModifiedAction[] { new ModifiedAction("RMB") };
            ChangeVariation = new ModifiedAction[] { new ModifiedAction("E") };
            Slot0 = new ModifiedAction[] { new ModifiedAction("0") };
            Slot1 = new ModifiedAction[] { new ModifiedAction("1") };
            Slot2 = new ModifiedAction[] { new ModifiedAction("2") };
            Slot3 = new ModifiedAction[] { new ModifiedAction("3") };
            Slot4 = new ModifiedAction[] { new ModifiedAction("4") };
            Slot5 = new ModifiedAction[] { new ModifiedAction("5") };
            Slot6 = new ModifiedAction[] { new ModifiedAction("6") };
            Slot7 = new ModifiedAction[] { new ModifiedAction("7") };
            Slot8 = new ModifiedAction[] { new ModifiedAction("8") };
            Slot9 = new ModifiedAction[] { new ModifiedAction("9") };
            NextWeapon = new ModifiedAction[] { new ModifiedAction("") };
            PrevWeapon = new ModifiedAction[] { new ModifiedAction("") };
            LastWeapon = new ModifiedAction[] { new ModifiedAction("Q") };
        }

        public static ModifiedActionMap GetBinds()
        {
            var json = File.ReadAllText($"{Plugin.GamePath}\\Preferences\\Binds.json");
            var actions = JsonConvert.DeserializeObject<UltrakillBindings>(json);
            var binds = new ModifiedActionMap();

            if (json == null) return binds;

            // scroll through all bind properties
            // scroll through all actions properties
            // if we get a match set bind property according to actions
            var bindsProperties = binds.GetType().GetProperties();
            for (int i = 0; i < bindsProperties.Length; i++)
            {
                foreach (var actionPropInfo in actions.Actions.GetType().GetProperties())
                {
                    var bindPropName = bindsProperties[i].GetCustomAttribute<JsonPropertyAttribute>().PropertyName;
                    var actionPropName = actionPropInfo.GetCustomAttribute<JsonPropertyAttribute>().PropertyName;

                    if (bindPropName == actionPropName)
                        bindsProperties[i].SetValue(binds, actionPropInfo.GetValue(actions.Actions));
                }
            }
            actions.Actions = binds;
            return actions.Actions;
        }
    }
}
