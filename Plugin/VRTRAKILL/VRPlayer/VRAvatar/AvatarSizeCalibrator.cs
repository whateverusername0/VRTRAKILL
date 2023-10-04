using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
{
    [RequireComponent(typeof(VRigController))]
    internal class AvatarSizeCalibrator : MonoBehaviour
    {
        private static AvatarSizeCalibrator _Instance; public static AvatarSizeCalibrator Instance { get { return _Instance; } }
        public MetaRig Rig => VRigController.Instance.Rig;

        public void Awake()
        {
            if (_Instance != null && _Instance != this) Destroy(this.gameObject);
            else _Instance = this;
        }


    }
}
