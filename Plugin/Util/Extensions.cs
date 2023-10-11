using Newtonsoft.Json;
using Plugin.VRTRAKILL.Config;
using System.IO;
using UnityEngine; using Object = UnityEngine.Object;

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
        /// <param name="Value"> Value of the setting (usually a <c>string</c> or a <c>bool</c>) </param>
        public static void ChangeWrite(this NewConfig Config, string SettingName, object Value)
        {
            Config.GetType().GetProperty(SettingName).SetValue(Config, Value);
            File.WriteAllText(ConfigMaster.ConfigPath, JsonConvert.SerializeObject(Config, Formatting.Indented));
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
