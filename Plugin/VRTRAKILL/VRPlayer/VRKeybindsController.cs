using UnityEngine;
using Plugin.VRTRAKILL.VRPlayer.VRCamera;

namespace Plugin.VRTRAKILL.VRPlayer
{
    // acts as many things in one
    internal class VRKeybindsController : MonoSingleton<VRKeybindsController>
    {
        private bool WasDVActive;

        public void Start()
        {
            if (Vars.Config.DesktopView.Enabled)
            {
                if (Vars.Config.DesktopView.ThirdPersonCamera.Enabled)
                    Vars.SpectatorCamera.SetActive(true);
                else Vars.DesktopCamera.gameObject.SetActive(true);
                Vars.DesktopUICamera.gameObject.SetActive(true);
            }

            if (ThirdPersonCamera.Instance != null)
                if (Vars.Config.DesktopView.ThirdPersonCamera.Mode <= 2 || Vars.Config.DesktopView.ThirdPersonCamera.Mode >= 0)
                    ThirdPersonCamera.Instance.Mode = (SCMode)Vars.Config.DesktopView.ThirdPersonCamera.Mode;
                else ThirdPersonCamera.Instance.Mode = 0;
        }

        public void Update()
        {
            if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.ToggleDesktopView))
            {
                SubtitleController.Instance.DisplaySubtitle("VR: Toggling desktop view");
                ToggleDesktopView();
            }
            if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.ToggleSpectatorCamera))
            {
                SubtitleController.Instance.DisplaySubtitle("VR: Toggling spectator camera");
                ToggleSpectatorCamera();
            }
            if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.EnumSpecCamMode))
            {
                ThirdPersonCamera.Instance.EnumSCMode();
                SubtitleController.Instance.DisplaySubtitle
                    ($"VR: Switched spectator camera mode to {System.Enum.GetName(typeof(SCMode), ThirdPersonCamera.Instance.Mode)}");
            }
            if (UnityEngine.Input.GetKeyDown((KeyCode)Config.ConfigMaster.ToggleAvatarCalibration))
            {
                try
                {
                    if ((bool)(VRAvatar.AvatarSizeCalibrator.Instance?.enabled))
                        VRAvatar.AvatarSizeCalibrator.Instance.enabled = false;
                    else if ((bool)!VRAvatar.AvatarSizeCalibrator.Instance?.enabled)
                        VRAvatar.AvatarSizeCalibrator.Instance.enabled = true;
                } catch (System.NullReferenceException) { SubtitleController.Instance.DisplaySubtitle("Unable to toggle ASC!"); }
            }
        }

        private void ToggleDesktopView()
        {
            Vars.SpectatorCamera.SetActive(false);

            if (!Vars.DesktopCamera.gameObject.activeSelf) Vars.DesktopCamera.gameObject.SetActive(true);
            else Vars.DesktopCamera.gameObject.SetActive(false);

            if (!Vars.DesktopUICamera.gameObject.activeSelf) Vars.DesktopUICamera.gameObject.SetActive(true);
            else if (Vars.DesktopUICamera.gameObject.activeSelf && !Vars.SpectatorCamera.activeSelf)
                Vars.DesktopUICamera.gameObject.SetActive(false);
        }
        private void ToggleSpectatorCamera()
        {
            

            if (!Vars.SpectatorCamera.activeSelf)
            {
                if (Vars.DesktopCamera.gameObject.activeSelf) { WasDVActive = true; Vars.DesktopCamera.gameObject.SetActive(false); }
                Vars.SpectatorCamera.SetActive(true);

                if (!Vars.DesktopUICamera.gameObject.activeSelf)
                    Vars.DesktopUICamera.gameObject.SetActive(true);
            }
            else
            {
                if (WasDVActive) { Vars.DesktopCamera.gameObject.SetActive(true); WasDVActive = false; }
                else Vars.DesktopUICamera.gameObject.SetActive(false);
                Vars.SpectatorCamera.gameObject.SetActive(false);
            }
        }
    }
}
