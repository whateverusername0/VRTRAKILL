using UnityEngine;
using UnityEngine.SceneManagement;

namespace Plugin.VRTRAKILL
{
    internal class Vars
    {
        public static bool NotAMenu =>
               !SceneManager.GetActiveScene().name.StartsWith("Main Menu")
            && !(OptionsManager.Instance != null && OptionsManager.Instance.paused)
            && !(SpawnMenu.Instance != null && SpawnMenu.Instance.gameObject.activeInHierarchy)
            && !(FinalRank.Instance != null && FinalRank.Instance.gameObject.activeInHierarchy);
          //&& alter menu active

        public static GameObject VRCameraContainer
        { get { return VRPlayer.VRCamera.Patches.CameraConverter.Container; } }
        private static Camera _MainCamera; public static Camera MainCamera
        {
            get { if (_MainCamera == null) { _MainCamera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>(); return _MainCamera; }
                  else return _MainCamera; }
        }
    }
}
