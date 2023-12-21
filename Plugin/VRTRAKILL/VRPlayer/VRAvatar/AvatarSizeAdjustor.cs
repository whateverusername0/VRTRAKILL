using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
{
    [RequireComponent(typeof(VRigController))]
    internal class AvatarSizeAdjustor : MonoBehaviour
    {
        private static AvatarSizeAdjustor _Instance; public static AvatarSizeAdjustor Instance { get { return _Instance; } }
        public MetaRig Rig => VRigController.Instance.Rig;

        private readonly float ScalePercentage = .05f;

        public void Awake()
        {
            if (_Instance != null && _Instance != this) Destroy(this.gameObject);
            else _Instance = this;
        }

        public void OnEnable()
        {
            SubtitleController.Instance.DisplaySubtitle("Avatar calibration is now enabled." +
                                                        "Press PrimaryFire to increase avatar's size," +
                                                        "press SecondaryFire to decrease.");
        }
        private bool IsHoldingKey = false;
        public void Update()
        {
            if (InputManager.Instance.InputSource.Fire1.WasPerformedThisFrame && !IsHoldingKey)
            { IsHoldingKey = true; ChangeSize(ScalePercentage, NewMovement.Instance.transform); }
            else if (InputManager.Instance.InputSource.Fire2.WasPerformedThisFrame && !IsHoldingKey)
            { IsHoldingKey = true; ChangeSize(-ScalePercentage, NewMovement.Instance.transform); }
            else if (InputManager.Instance.InputSource.Fire1.WasCanceledThisFrame
                 || InputManager.Instance.InputSource.Fire2.WasCanceledThisFrame
                 && IsHoldingKey) IsHoldingKey = false;
        }
        public void OnDisable()
        {
            SubtitleController.Instance.DisplaySubtitle("Avatar calibration is now disabled.");
        }

        private void ChangeSize(float ScalePercentage, Transform Target)
        {
            Target.localScale = new Vector3(Target.localScale.x + ScalePercentage,
                                            Target.localScale.y + ScalePercentage,
                                            Target.localScale.z + ScalePercentage);
        }
    }
}
