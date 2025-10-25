using VRTRAKILL.Data;
using UnityEngine;

namespace VRTRAKILL.Systems.VRPlayer
{
    // acts as many things in one
    public class VRKeybindsController : MonoSingleton<VRKeybindsController>
    {
        public static bool WasDVActive { get; set; }

        public void Start() { /* used as a placeholder for extensions */ }

        public void Update()
        {
            if (UnityEngine.Input.GetKeyDown((KeyCode)Prefs.ConfigMaster.ToggleDesktopView))
            {
                SubtitleController.Instance.DisplaySubtitle("VR: Toggling desktop view");
                ToggleDesktopView();
            }
            if (UnityEngine.Input.GetKeyDown((KeyCode)Prefs.ConfigMaster.ToggleAvatarSizeAdj))
            {
                try
                {
                    if ((bool)(VRAvatar.AvatarSizeAdjustor.Instance?.enabled))
                        VRAvatar.AvatarSizeAdjustor.Instance.enabled = false;
                    else if ((bool)!VRAvatar.AvatarSizeAdjustor.Instance?.enabled)
                        VRAvatar.AvatarSizeAdjustor.Instance.enabled = true;
                } catch (System.NullReferenceException) { SubtitleController.Instance.DisplaySubtitle("Unable to toggle avatar size adjustment!"); }
            }
        }

        public void ToggleDesktopView()
        {
            if (!Vars.DesktopCamera.activeSelf) Vars.DesktopCamera.SetActive(true);
            else Vars.DesktopCamera.SetActive(false);

            if (!Vars.DesktopUICamera.activeSelf) Vars.DesktopUICamera.SetActive(true);
            else if (Vars.DesktopUICamera.activeSelf)
                Vars.DesktopUICamera.SetActive(false);
        }
    }
}
