using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer
{
    internal class VRController : MonoSingleton<VRController>
    {
        public void Update()
        {
            if (UnityEngine.Input.GetKeyDown(Vars.Config.VRKeybinds.ToggleDV))
            {
                SubtitleController.Instance.DisplaySubtitle("VR: Enabling desktop view");
                Vars.SpectatorCamera.SetActive(false);
                Vars.DesktopCamera.gameObject.SetActive(true);
            }
            if (UnityEngine.Input.GetKeyDown(Vars.Config.VRKeybinds.ToggleSC))
            {
                SubtitleController.Instance.DisplaySubtitle("VR: Enabling spectator camera");
                Vars.SpectatorCamera.SetActive(true);
                Vars.DesktopCamera.gameObject.SetActive(false);
            }
            if (UnityEngine.Input.GetKeyDown(Vars.Config.VRKeybinds.EnumSCMode))
            {
                VRCamera.SpectatorCamera.Instance.EnumSCMode();
                SubtitleController.Instance.DisplaySubtitle($"VR: Switched spectator camera mode to {nameof(VRCamera.SpectatorCamera.Mode)}");
            }

        }
    }
}
