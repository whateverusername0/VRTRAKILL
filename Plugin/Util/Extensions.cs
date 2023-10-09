using Newtonsoft.Json;
using Plugin.VRTRAKILL.Config;
using System.IO;
using UnityEngine; using Object = UnityEngine.Object;

namespace Plugin.Util
{
    internal static class Extensions
    {
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

        public static void ChangeWrite(this NewConfig Config, string SettingName, object Value)
        {
            Config.GetType().GetProperty(SettingName).SetValue(Config, Value);
            File.WriteAllText(ConfigMaster.ConfigPath, JsonConvert.SerializeObject(Config, Formatting.Indented));
        }
        public static KeyCode ToKeyCode(this object O)
        {
            if (VRTRAKILL.Input.InputMap.UKeys.TryGetValue(O.ToString(), out KeyCode? A))
                return (KeyCode)A;
            else return 0;
        }
    }
}
