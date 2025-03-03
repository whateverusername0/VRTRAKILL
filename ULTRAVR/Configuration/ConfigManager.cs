using Newtonsoft.Json;
using System.IO;

namespace ULTRAVR.Configuration
{
    public static class ConfigManager
    {
        public static JSONNewConfig Instance
        {
            get
            {
                if (Instance == null) Instance = Deserialize();
                return Instance;
            }
            private set { Instance = value; }
        }
        public static KeybindMap Bindings { get; private set; }

        #region VRTRAKILL Config
        
        #endregion

        #region ULTRAKILL Keybindings

        #endregion
    }
}
