using Plugin.VRTRAKILL.VRPlayer.Controllers;
using Plugin.Util;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
{
    internal class VRigController : MonoBehaviour
    {
        private static VRigController _Instance; public static VRigController Instance { get { return _Instance; } }

        public MetaRig Rig; public GameObject Head;
        public Vector3 HeadOffsetPosition = new Vector3(0, 0, 0),
                       HeadOffsetAngles = new Vector3(-90, 0, 0);

        private IKArm AddArmIK(GameObject GO, Transform Target, int ChainLen = 3, Transform Pole = null)
        {
            IKArm IK = GO.AddComponent<IKArm>();
            IK.Target = Target; IK.ChainLength = ChainLen; IK.Pole = Pole;
            return IK;
        }

        public void Awake()
        {
            if (_Instance != null && _Instance != this) Destroy(this.gameObject);
            else _Instance = this;
        }

        public void Start()
        {
            if (Rig == null) Rig = MetaRig.CreateVCustomPreset(Vars.VRCameraContainer, "VR Avatar");
            if (Head == null) Head = Instantiate(Assets.Vars.VHead, Vars.VRCameraContainer.transform);
            Misc.RecursiveChangeLayer(Rig.GameObjectT.gameObject, (int)Layers.AlwaysOnTop);

            // transform shenanigans
            Rig.GameObjectT.localPosition = Vector3.zero;
            Rig.GameObjectT.localRotation = Quaternion.Euler(Vector3.zero);

            Rig.Root.localScale *= 3;
            Rig.Body.localPosition = new Vector3(0, -0.0135f, 0.002f);

            // Arm IKs
            Armature.Arm[] LArms =
            {
                Rig._LFeedbacker, Rig._LKnuckleblaster,
                Rig._LWhiplash, Rig._LSandboxer,
            };
            foreach (Armature.Arm Arm in LArms)
                AddArmIK(Arm.Hand.Root.gameObject, ArmController.Instance.CC.ArmOffset.transform, Pole: Rig.IKPole_Left);

            Armature.Arm[] RArms =
            {
                Rig._RFeedbacker, Rig._RKnuckleblaster,
                Rig._RWhiplash, Rig._RSandboxer,
            };
            foreach (Armature.Arm Arm in RArms)
                AddArmIK(Arm.Hand.Root.gameObject, GunController.Instance.CC.ArmOffset.transform, Pole: Rig.IKPole_Right);

            // Leg IKs TBD

            //gameObject.AddComponent<SkinsManager>();
        }
        
        public void LateUpdate()
        {
            
            if (Rig == null) return;

            if (!Vars.IsPlayerFrozen && !Vars.IsMainMenu)
            {
                HandleBodyRotation();
                HandleHeadRotation();
                HandleArms();
            }
        }

        private void HandleBodyRotation()
        {
            Rig.Root.position = Vars.MainCamera.transform.position;
            if ((Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Abdomen.rotation.eulerAngles.y) >= Quaternion.Euler(0, 90, 0).y
            || (Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Abdomen.rotation.eulerAngles.y) <= Quaternion.Euler(0, -90, 0).y)
            {
                Quaternion Rotation = Quaternion.Lerp(Rig.Abdomen.rotation, Vars.MainCamera.transform.rotation, Time.deltaTime * 2.5f);
                Rig.Root.rotation = Quaternion.Euler(0, Rotation.eulerAngles.y, 0);
            }
        }
        private void HandleHeadRotation()
        {
            Head.transform.GetChild(0).GetChild(0).position = Vars.MainCamera.transform.position + HeadOffsetPosition;
            Head.transform.GetChild(0).GetChild(0).eulerAngles = Vars.MainCamera.transform.eulerAngles + HeadOffsetAngles;
        }
        private void HandleArms()
        {
            // Main Menu
            if (Vars.IsMainMenu)
            {
                Rig.FeedbackerA.GameObjecT.gameObject.SetActive(true);
                Rig.FeedbackerB.GameObjecT.gameObject.SetActive(true);
                Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(false);
                Rig.Whiplash.GameObjecT.gameObject.SetActive(false);
                Rig.Sandboxer.GameObjecT.gameObject.SetActive(false);
            }

            // Arm to render
            if (GunControl.Instance != null
            && GunControl.Instance.currentWeapon != null
            && GunControl.Instance.currentWeapon.HasComponent<Sandbox.Arm.SandboxArm>())
            {
                Rig.FeedbackerB.GameObjecT.gameObject.SetActive(false);
                Rig.Sandboxer.GameObjecT.gameObject.SetActive(true);
            }
            else
            {
                Rig.FeedbackerB.GameObjecT.gameObject.SetActive(true);
                Rig.Sandboxer.GameObjecT.gameObject.SetActive(false);
            }

            Armature.Arm ActiveArm = null;
            switch (FistControl.Instance.currentPunch.type)
            {
                case FistType.Standard:
                    ActiveArm = Rig.FeedbackerA;
                    Rig.FeedbackerA.GameObjecT.gameObject.SetActive(true);
                    Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(false);
                    break;
                case FistType.Heavy:
                    ActiveArm = Rig.Knuckleblaster;
                    Rig.FeedbackerA.GameObjecT.gameObject.SetActive(false);
                    Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(true);
                    break;
                case FistType.Spear:
                default: break;
            }

            if (HookArm.Instance != null && HookArm.Instance.enabled
            && HookArm.Instance.model.activeSelf && !Vars.Config.MBP.CameraWhiplash)
            {
                ActiveArm.GameObjecT.gameObject.SetActive(false);
                Rig.Whiplash.GameObjecT.gameObject.SetActive(true);
            }
            else
            {
                ActiveArm.GameObjecT.gameObject.SetActive(true);
                Rig.Whiplash.GameObjecT.gameObject.SetActive(false);
            }
        }
    }
}
