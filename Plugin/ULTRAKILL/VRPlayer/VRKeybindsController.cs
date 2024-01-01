using UnityEngine;
using VRBasePlugin.ULTRAKILL.VRPlayer.VRCamera;

namespace VRBasePlugin.ULTRAKILL.VRPlayer
{
    // acts as many things in one
    public class VRKeybindsController : MonoSingleton<VRKeybindsController>
    {
        public static bool WasDVActive { get; set; }

        public void Start() { /* used as a placeholder for extensions */ }

        public void Update()
        {
            if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.ToggleDesktopView))
            {
                SubtitleController.Instance.DisplaySubtitle("VR: Toggling desktop view");
                ToggleDesktopView();
            }
            if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.ToggleAvatarSizeAdj))
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
            if (!Vars.DesktopCamera.gameObject.activeSelf) Vars.DesktopCamera.gameObject.SetActive(true);
            else Vars.DesktopCamera.gameObject.SetActive(false);

            if (!Vars.DesktopUICamera.gameObject.activeSelf) Vars.DesktopUICamera.gameObject.SetActive(true);
            else if (Vars.DesktopUICamera.gameObject.activeSelf)
                Vars.DesktopUICamera.gameObject.SetActive(false);
        }
    }
}
