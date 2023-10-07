using Plugin.VRTRAKILL.VRPlayer.Controllers;
using Plugin.Util;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
{
    internal class VRigController : MonoBehaviour
    {
        private static VRigController _Instance; public static VRigController Instance { get { return _Instance; } }

        public MetaRig Rig;
        public Vector3 HeadOffsetPosition = new Vector3(0, 0, 0),
                       HeadOffsetAngles = new Vector3(-90, 0, 0);

        private IKChain AddIK(GameObject GO, Transform Target, int ChainLen = 3, Transform Pole = null)
        {
            IKChain IK = GO.AddComponent<IKChain>();
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
            Misc.RecursiveChangeLayer(Rig.GameObjectT.gameObject, (int)Layers.AlwaysOnTop);

            // transform shenanigans
            Rig.GameObjectT.localPosition = Vector3.zero;
            Rig.GameObjectT.localRotation = Quaternion.Euler(Vector3.zero);

            Rig.Root.localScale *= 3;
            Rig.Body.localPosition = new Vector3(0, -.0138f, .0025f);

            // Arm IKs
            Armature.Arm[] LArms =
            {
                Rig._LFeedbacker, Rig._LKnuckleblaster,
                Rig._LWhiplash, Rig._LSandboxer,
            };
            foreach (Armature.Arm Arm in LArms)
                AddIK(Arm.Hand.Root.gameObject, ArmController.Instance.CC.ArmOffset.transform, Pole: Rig.IKPole_Left);

            Armature.Arm[] RArms =
            {
                Rig._RFeedbacker, Rig._RKnuckleblaster,
                Rig._RWhiplash, Rig._RSandboxer,
            };
            foreach (Armature.Arm Arm in RArms)
                AddIK(Arm.Hand.Root.gameObject, GunController.Instance.CC.ArmOffset.transform, Pole: Rig.IKPole_Right);

            // Leg IKs TBD
            // add blabla

            AddIK(Rig.NeckEnd.gameObject, Rig.Head.GetChild(0).GetChild(0), 2);

            //gameObject.AddComponent<SkinsManager>();
            //var ASC = gameObject.AddComponent<AvatarSizeCalibrator>(); ASC.enabled = false;
        }
        
        public void LateUpdate()
        {
            if (Rig == null) return;

            HandleBodyRotation();
            HandleHeadRotation();

            if (!Vars.IsMainMenu)
                HandleArms();
            else
            {
                Rig.FeedbackerA.GameObjecT.gameObject.SetActive(true);
                Rig.FeedbackerB.GameObjecT.gameObject.SetActive(true);
                Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(false);
                Rig.Whiplash.GameObjecT.gameObject.SetActive(false);
                Rig.Sandboxer.GameObjecT.gameObject.SetActive(false);
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
            Rig.Head.GetChild(0).position = Vars.MainCamera.transform.position;
            Rig.Head.GetChild(0).eulerAngles = Vars.MainCamera.transform.eulerAngles;
        }
        private void HandleArms()
        {
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
                default:
                    ActiveArm = Rig.FeedbackerA;
                    Rig.FeedbackerA.GameObjecT.gameObject.SetActive(true);
                    Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(false);
                    break;
                case FistType.Heavy:
                    ActiveArm = Rig.Knuckleblaster;
                    Rig.FeedbackerA.GameObjecT.gameObject.SetActive(false);
                    Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(true);
                    break;
                case FistType.Spear: break;
            }

            // Whiplash
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
