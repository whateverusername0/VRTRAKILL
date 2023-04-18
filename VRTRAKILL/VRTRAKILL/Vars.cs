using UnityEngine;

namespace Plugin.VRTRAKILL
{
    internal class Vars
    {
        public static GameObject VRCameraContainer
        {
            get { return VRPlayer.VRCamera.Patches.CameraConverter.Container; }
            set { VRPlayer.VRCamera.Patches.CameraConverter.Container = value; }
        }
        private static Camera _MainCamera; public static Camera MainCamera
        {
            get { if (_MainCamera == null) { _MainCamera = GameObject.Find("Main Camera").gameObject.GetComponent<Camera>(); return _MainCamera; }
                  else return _MainCamera; }
        }
    }
}
