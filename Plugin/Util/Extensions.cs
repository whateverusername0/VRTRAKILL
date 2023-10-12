using Newtonsoft.Json;
using Plugin.VRTRAKILL.Config;
using System.IO;
using UnityEngine; using Object = UnityEngine.Object;
using Plugin.VRTRAKILL;

namespace Plugin.Util
{
    internal static class Extensions
    {
        /// <summary>
        /// Checks if a <c>Component</c> is present in GM
        /// </summary>
        /// <typeparam name="T"> The <c>Component</c> class (e.g. <c>NewMovement</c>) </typeparam>
        /// <param name="GM"> An instance of a <c>GameObject</c> </param>
        /// <returns></returns>
        public static bool HasComponent<T>(this GameObject GM) where T : Component
        { return GM.GetComponent<T>() != null; }

        /// <summary>
        /// Converts a string to a Version
        /// </summary>
        /// <param name="sVersion"> Version as a string </param>
        /// <returns></returns>
        public static Version ToVersion(this string sVersion)
        {
            string[] Chars = sVersion.Split('.', '-', ' ');
            return new Version(int.Parse(Chars[0]), int.Parse(Chars[1]), int.Parse(Chars[2]));
        }

        /// <summary>
        /// Make changes to the config and write to file
        /// </summary>
        /// <param name="Config"> An instance of the <c>NewConfig</c> class </param>
        /// <param name="SettingName"> A name of the setting (usually set by <c>nameof()</c>) </param>
        /// <param name="Value"> Value of the setting </param>
        public static void ChangeWrite<T>(this NewConfig Config, string SettingName, T Value)
        {
            Vars.Log.LogInfo($"Writing changes to {SettingName}");
            if (Value.GetType() == Config.GetType().GetProperty(SettingName).GetType())
                Config.GetType().GetProperty(SettingName).SetValue(Config, Value);
            else if (Config.GetType().GetProperty(SettingName).GetType() == typeof(string))
                Config.GetType().GetProperty(SettingName).SetValue(Config, Value.ToString());
            else throw new System.Exception("Type mismatch!");
            
            File.WriteAllText(ConfigMaster.ConfigPath, JsonConvert.SerializeObject(ConfigJSON.Instance, Formatting.Indented));
            Vars.Log.LogInfo($"Successfully written changes to {SettingName}");
        }
        /// <summary>
        /// Converts an object (usually a <c>string</c>) to a <c>UnityEngine.KeyCode</c>
        /// </summary>
        /// <param name="O"></param>
        /// <returns></returns>
        public static KeyCode ToKeyCode(this object O)
        {
            if (VRTRAKILL.Input.InputMap.UKeys.TryGetValue(O.ToString(), out KeyCode? A))
                return (KeyCode)A;
            else return 0;
        }
    }
}
