using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;

namespace Plugin
{
    [BepInPlugin("com.popikman.vrtrakill", "VRTRAKILL", "0.1")]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource PLogger { get; private set; }

        private void Awake()
        {
            AppDomain.CurrentDomain.UnhandledException += CustomExceptionHandler;
            PLogger = Logger;
            HarmonyLib.Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());


        }

        private void CustomExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception E = e.ExceptionObject as Exception;
            PLogger.LogError($"Unknown error at {E.Source}\n Details: {E.Message}");
        }
    }
}