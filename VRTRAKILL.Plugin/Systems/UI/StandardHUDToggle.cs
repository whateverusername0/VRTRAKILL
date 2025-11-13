using UnityEngine;

namespace VRTRAKILL.Systems.UI;

// toggles standard hud on and off.
// yea for some reason it won't work any other way.
public class StandardHUDToggle : MonoBehaviour
{
    public GameObject StandardHUD;

    public void Update()
    {
        if (StandardHUD == null) return;

        if (MonoSingleton<PrefsManager>.Instance.GetInt("hudType") != 1) StandardHUD.SetActive(false);
        else StandardHUD.SetActive(true);
    }
}