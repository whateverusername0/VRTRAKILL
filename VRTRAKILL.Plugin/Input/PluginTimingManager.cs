using UnityEngine.XR;

namespace VRTRAKILL.Plugin.Input
{
    [ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
    public class PluginTimingManager : MonoSingleton<PluginTimingManager>
    {
        private Plugin _plugin => Plugin.Instance;

        protected XRDisplaySubsystem XRDisplay;

        public static float DefaultRefreshRate = 72f;

        public static float Multiplier { get; private set; } = 1f;

        protected void Awake()
        {
            if (_plugin == null || _plugin.OpenVRLoader == null)
                return;

            XRDisplay = _plugin.OpenVRLoader.GetLoadedSubsystem<XRDisplaySubsystem>();
        }

        protected void Update()
        {
            if (XRDisplay == null)
            {
                Awake();
                return;
            }

            if (!XRDisplay.TryGetDisplayRefreshRate(out var rr))
                return;

            if (rr == 0f)
            {
                _plugin.Log.LogWarning("Unable to get the HMD refresh rate. Defaulting to 72Hz.");
                rr = DefaultRefreshRate;
            }

            // the evil fucking magic number of doom
            // if you divide 1 by it you would get 125.2175 something
            // which is supposedly ultrakill's fixed timestamp
            // we REALLY want it to stay stable
            // so that the player does not jump 65 feet into the air on a valve index.
            Multiplier = 0.0079861f * rr;
        }
    }
}
