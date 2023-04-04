using System;
using BepInEx;
using Valve.VR;

namespace Plugin
{
    [BepInPlugin("com.popikman.vrtrakill", "ULTRAKILLing in VR is now a thing.", "0.1")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.Log(0, $"VRTRAKILL is loaded, initializing SteamVR");
            InitSteamVR();
        }

        private static void InitSteamVR()
        {
            SteamVR.Initialize();
            SteamVR_Settings.instance.pauseGameWhenDashboardVisible = true;
        }
    }
}