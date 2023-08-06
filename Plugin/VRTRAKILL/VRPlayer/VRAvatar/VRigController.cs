using Plugin.VRTRAKILL.VRPlayer.Controllers;
using Plugin.Helpers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
{
    internal class VRigController : MonoBehaviour
    {
        private static VRigController _Instance; public static VRigController Instance { get { return _Instance; } }

        public MetaRig Rig;

        private Armature.Arm _ActiveArm; public Armature.Arm ActiveArm
        {
            get { return _ActiveArm; }
            set
            {
                _ActiveArm?.GameObjecT.gameObject.SetActive(false);
                _ActiveArm = value;
                _ActiveArm.GameObjecT.gameObject.SetActive(true);
            }
        }

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
            if (Rig == null) Rig = MetaRig.CreateVCustomPreset(Vars.VRCameraContainer);
            Misc.RecursiveChangeLayer(Rig.GameObjectT.gameObject, (int)Vars.Layers.AlwaysOnTop);

            // transform shenanigans
            Rig.GameObjectT.localPosition = Vector3.zero;
            Rig.GameObjectT.localRotation = Quaternion.Euler(Vector3.zero);

            Rig.Root.localScale *= 3;
            Rig.Body.localPosition = new Vector3(0, -0.0125f, 0.002f);

            Armature.Arm[] Arms =
            {
                Rig._LFeedbacker, Rig._LKnuckleblaster,
                Rig._LWhiplash, Rig._LSandboxer,
                Rig._RFeedbacker, Rig._RKnuckleblaster,
                Rig._RWhiplash, Rig._RSandboxer,
            };
            // Arm IKs
            foreach(Armature.Arm Arm in Arms)
                AddArmIK(Arm.Hand.Root.gameObject, ArmController.Instance.CC.ArmOffset.transform, Pole: Arm.Pole);

            // Leg IKs TBD

            gameObject.AddComponent<SkinsManager>();
        }
        
        public void LateUpdate()
        {
            if (Rig == null) return;

            if (Vars.IsPaused && !Vars.IsAMenu)
                Rig.Head.localScale = Vector3.one;
            else
            {
                Rig.Head.localScale = Vector3.zero;

                HandleBodyRotation();
                HandleHeadRotation();
                HandleArms();
                MoveIKPoles();
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
            Rig.Head.position = Vars.MainCamera.transform.position;
            Rig.Head.rotation = Vars.MainCamera.transform.rotation * Quaternion.Euler(-45, 0, 0);
        }
        private void HandleArms()
        {
            // main menu
            if (Vars.IsMainMenu)
            {
                Rig._LFeedbacker.GameObjecT.gameObject.SetActive(true);
                Rig._RFeedbacker.GameObjecT.gameObject.SetActive(true);
                Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(false);
                Rig.Whiplash.GameObjecT.gameObject.SetActive(false);
                Rig.Sandboxer.GameObjecT.gameObject.SetActive(false);
            }

            // Determine which arm to render
            if (GunControl.Instance.currentWeapon.HasComponent<Sandbox.Arm.SandboxArm>())
                ActiveArm = Rig.Sandboxer;
            else switch(FistControl.Instance.currentPunch.type)
            {
                case FistType.Standard:
                    ActiveArm = Rig.Feedbacker; break;
                case FistType.Heavy:
                    ActiveArm = Rig.Knuckleblaster; break;
                case FistType.Spear:
                default: break;
            }
            if (HookArm.Instance != null && HookArm.Instance.enabled && HookArm.Instance.model.activeSelf)
            {
                Rig.Feedbacker.GameObjecT.gameObject.SetActive(false);
                Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(false);
                Rig.Whiplash.GameObjecT.gameObject.SetActive(true);
            }
            else Rig.Whiplash.GameObjecT.gameObject.SetActive(false);
        }
        private void MoveIKPoles()
        {
            Vector3 Position = Vars.DominantHand.transform.forward * -5;
            Rig._LFeedbacker.Pole.position = Position;
            Rig._RFeedbacker.Pole.position = Position;

            Rig.Knuckleblaster.Pole.position = Position;
            Rig.Whiplash.Pole.position = Position;
            Rig.Sandboxer.Pole.position = Position;
        }
    }
}
