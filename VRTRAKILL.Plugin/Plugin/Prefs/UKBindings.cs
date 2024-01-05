using Newtonsoft.Json;
using System.IO;
using System.Reflection;

namespace VRBasePlugin.Prefs
{
    /// <summary>
    /// Represents the Binds.json file located in ULTRAKILL\\Preferences\\Binds.json
    /// </summary>
    public class UKBindings
    {
        /// <summary>
        /// Represents the modifiedActions object in Binds.json
        /// </summary>
        public class ModifiedActions
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
            /// Creates a new <see cref="ModifiedActions"/> class with the default game parameters.
            /// </summary>
            public ModifiedActions()
            {
                Dodge           = new ModifiedAction[] { new ModifiedAction("LeftShift") };
                Slide           = new ModifiedAction[] { new ModifiedAction("LeftControl") };
                Jump            = new ModifiedAction[] { new ModifiedAction("Space") };
                ChangeFist      = new ModifiedAction[] { new ModifiedAction("G") };
                Punch           = new ModifiedAction[] { new ModifiedAction("F") };
                Hook            = new ModifiedAction[] { new ModifiedAction("R") };
                PrimaryFire     = new ModifiedAction[] { new ModifiedAction("LMB") };
                SecondaryFire   = new ModifiedAction[] { new ModifiedAction("RMB") };
                ChangeVariation = new ModifiedAction[] { new ModifiedAction("E") };
                Slot0           = new ModifiedAction[] { new ModifiedAction("0") };
                Slot1           = new ModifiedAction[] { new ModifiedAction("1") };
                Slot2           = new ModifiedAction[] { new ModifiedAction("2") };
                Slot3           = new ModifiedAction[] { new ModifiedAction("3") };
                Slot4           = new ModifiedAction[] { new ModifiedAction("4") };
                Slot5           = new ModifiedAction[] { new ModifiedAction("5") };
                Slot6           = new ModifiedAction[] { new ModifiedAction("6") };
                Slot7           = new ModifiedAction[] { new ModifiedAction("7") };
                Slot8           = new ModifiedAction[] { new ModifiedAction("8") };
                Slot9           = new ModifiedAction[] { new ModifiedAction("9") };
                NextWeapon      = new ModifiedAction[] { new ModifiedAction("") };
                PrevWeapon      = new ModifiedAction[] { new ModifiedAction("") };
                LastWeapon      = new ModifiedAction[] { new ModifiedAction("Q") };
            }
        }
        /// <summary>
        /// Represents a modified action object of Binds.json.
        /// If it's an array, use only the zeroth index.
        /// </summary>
        public class ModifiedAction
        {
            [JsonProperty("path")] public string LocalPath { get; set; }
            [JsonIgnore] public string Path => GetPath();
            public string GetPath()
            {
                if (LocalPath.Contains("/"))
                {
                    string[] Temp = LocalPath.Split('/');
                    return Temp[Temp.Length - 1];
                }
                return LocalPath;
            }
            public ModifiedAction(string LocalPath)
            {
                this.LocalPath = LocalPath;
            }
        }
        [JsonProperty("controlScheme")] public string ControlScheme { get; set; } = "Keyboard & Mouse";
        [JsonProperty("modifiedActions")] public ModifiedActions Actions { get; set; }

        public static UKBindings GetBinds()
        {
            UKBindings Json = JsonConvert.DeserializeObject<UKBindings>(File.ReadAllText($"{PluginInfo.GamePath}\\Preferences\\Binds.json"));
            var Binds = new ModifiedActions();

            // What this does is basically scroll thru all the properties in the Binds object
            // For each property in Binds scroll through all the properties in the Json.Actions obejct
            // If it has the matching names (as stated in the JsonProperty) and different values,
            // swap the default value with the .json one.
            PropertyInfo[] PropInfo = Binds.GetType().GetProperties();
            for (int i = 0; i < PropInfo.Length; i++)
            {
                foreach (PropertyInfo JsonPropInfo in Json.Actions.GetType().GetProperties())
                    if (PropInfo[i].GetCustomAttribute<JsonPropertyAttribute>().PropertyName == JsonPropInfo.GetCustomAttribute<JsonPropertyAttribute>().PropertyName
                    && PropInfo[i].GetValue(Binds) != JsonPropInfo.GetValue(Json.Actions))
                        PropInfo[i].SetValue(Binds, JsonPropInfo.GetValue(Json.Actions));
            }
            Json.Actions = Binds;
            return Json;
        }
    }
}
