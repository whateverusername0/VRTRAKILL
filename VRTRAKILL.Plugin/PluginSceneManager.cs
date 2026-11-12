using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;
using VRTRAKILL.Plugin.Utilities;

namespace VRTRAKILL.Plugin;

[ConfigureSingleton(SingletonFlags.PersistAutoInstance)]
public class PluginSceneManager : MonoSingleton<PluginSceneManager>
{
    private Plugin _plugin => Plugin.Instance;

    protected void Awake()
        => SceneManager.activeSceneChanged += OnSceneChanged;

    protected void OnDestroy()
        => SceneManager.activeSceneChanged -= OnSceneChanged;

    private void OnSceneChanged(Scene arg0, Scene arg1)
    {
        // unsure if this is necessary
        // will leave it just in case

        if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null)
        {
            _plugin.Log.LogError("XR General Settings are null!");
            return;
        }

        var loader = XRGeneralSettings.Instance.Manager.activeLoader;
        if (loader == null)
            XRGeneralSettings.Instance.Manager.InitializeLoaderSync();

        UnityExtensions.EnableOffscreenRendering();
    }
}