using Plugin.VRTRAKILL.VRPlayer.Controllers;
using Plugin.Util;
using Plugin.Util.Libraries.EZhex1991.EZSoftBone;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRAvatar
{
    // the NewMovement of the player's avatar
    internal class VRigController : MonoBehaviour
    {
        // can't use stuff normally without it being both a singleton and a monobehavior
        private static VRigController _Instance; public static VRigController Instance { get { return _Instance; } }

        public MetaRig Rig; private EZSoftBone WingBone;
        public Vector3 HeadOffsetPosition = new Vector3(0, 0, 0),
                       HeadOffsetAngles = new Vector3(-90, 0, 0);
        public Animator Anim;

        private IKChain AddIK(GameObject GO, Transform Target, int ChainLen = 2, Transform Pole = null)
        {
            IKChain IK = GO.AddComponent<IKChain>();
            IK.Target = Target; IK.ChainLength = ChainLen; IK.Pole = Pole;
            return IK;
        }

        public void Awake()
        {
            // awake would be inaccessible if it was a monosingleton
            if (_Instance != null && _Instance != this) Destroy(this.gameObject);
            else _Instance = this;
        }

        public void Start()
        {
            Rig = Rig ?? MetaRig.CreateVCustomPreset(Vars.VRCameraContainer, "VR Avatar");
            Util.Unity.RecursiveChangeLayer(Rig.GameObjectT.gameObject, (int)Layers.AlwaysOnTop);

            // transform shenanigans (necessary)
            Rig.GameObjectT.localPosition = Vector3.zero;
            Rig.GameObjectT.localRotation = Quaternion.Euler(Vector3.zero);

            Rig.Root.localScale *= 3;
            Rig.Root.GetChild(0).localPosition = new Vector3(0, -.015f, -.0005f);


            if (Vars.Config.VRBody.EnableArmsIK)
            {
                Armature.Arm[] LArms =
                {
                    Rig._LFeedbacker, Rig._LKnuckleblaster,
                    Rig._LWhiplash, Rig._LSandboxer,
                };
                foreach (Armature.Arm Arm in LArms)
                    AddIK(Arm.Hand.Root.gameObject, ArmController.Instance.CC.ArmOffset.transform, Pole: Rig.Arm_IKPole_Left);

                Armature.Arm[] RArms =
                {
                    Rig._RFeedbacker, Rig._RKnuckleblaster,
                    Rig._RWhiplash, Rig._RSandboxer,
                };
                foreach (Armature.Arm Arm in RArms)
                    AddIK(Arm.Hand.Root.gameObject, GunController.Instance.CC.ArmOffset.transform, Pole: Rig.Arm_IKPole_Right);
            }

            if (Vars.Config.VRBody.EnableLegsIK)
            {
                Anim = Rig.GameObjectT.GetComponent<Animator>();
                AddIK(Rig.LeftLegIK.Foot.gameObject, Rig.LeftLeg.Foot, Pole: Rig.Leg_IKPole_Left);
                AddIK(Rig.RightLegIK.Foot.gameObject, Rig.RightLeg.Foot, Pole: Rig.Leg_IKPole_Right);

                IKFoot LeftLeg = Rig.LeftLeg.Foot.gameObject.AddComponent<IKFoot>();
                LeftLeg.Anim = this.Anim;
                LeftLeg.Body = Rig.Root;
                LeftLeg.FootSpacing = -.3f;
                IKFoot RightLeg = Rig.RightLeg.Foot.gameObject.AddComponent<IKFoot>();
                RightLeg.Anim = this.Anim;
                RightLeg.Body = Rig.Root;
                RightLeg.FootSpacing = .3f;

                LeftLeg.OtherFoot = RightLeg;
                RightLeg.OtherFoot = LeftLeg;
            }

            #region Dynamic wings

            WingBone = Rig.Chest.GetChild(4).gameObject.AddComponent<EZSoftBone>();
            WingBone.m_RootBones.Add(WingBone.transform);
            WingBone.startDepth = 1;
            WingBone.collisionLayers = 1 << (int)Layers.Environment;
            WingBone.deltaTimeMode = EZSoftBone.DeltaTimeMode.Constant;
            WingBone.constantDeltaTime = 5;
            WingBone.Awake();
            WingBone.material.damping = 1;
            WingBone.material.stiffness = .8f;
            WingBone.material.resistance = 0;
            WingBone.material.slackness = 0;

            #endregion

            // Neck chain Ik
            AddIK(Rig.NeckEnd.gameObject, Rig.Head.GetChild(0).GetChild(0), 2);

            this.gameObject.SetActive(false);
            var ASC = gameObject.AddComponent<AvatarSizeAdjustor>();
            ASC.enabled = false;
            this.gameObject.SetActive(true);
        }
        
        public void LateUpdate()
        {
            if (Rig == null) return;

            HandleBodyTransform();
            HandleHead();
            HandleArms();
            HandleWings();
            if (Vars.Config.VRBody.EnableLegsIK)
            {
                HandleAnimations();
                HandlePelvisRotation();
            }
        }

        private void HandleBodyTransform()
        {
            // Smooth body's position and rotation towards the camera
            Rig.Root.position = Vector3.Lerp(Rig.Root.position, Vars.MainCamera.transform.position, Time.deltaTime * 5f);
            Quaternion Rotation = Quaternion.Lerp(Rig.Abdomen.rotation, Vars.MainCamera.transform.rotation, Time.deltaTime * 2.5f);
            Rig.Root.rotation = Quaternion.Euler(0, Rotation.eulerAngles.y, 0);
        }
        private void HandleHead()
        {
            Rig.Head.GetChild(0).position = Vars.MainCamera.transform.position;
            Rig.Head.GetChild(0).eulerAngles = Vars.MainCamera.transform.eulerAngles;
        }
        private void HandleArms()
        {
            // This method decides which arm to render

            // Reset everything to feedbacker in the main menu
            if (Vars.IsMainMenu)
            {
                Rig.FeedbackerA.GameObjecT.gameObject.SetActive(true);
                Rig.FeedbackerB.GameObjecT.gameObject.SetActive(true);
                Rig.Knuckleblaster.GameObjecT.gameObject.SetActive(false);
                Rig.Whiplash.GameObjecT.gameObject.SetActive(false);
                Rig.Sandboxer.GameObjecT.gameObject.SetActive(false);
                return;
            }

            // Sandbox arm
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

            // Set activearm to use with whiplash (for convenience)
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

            // Whiplash behavior
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

        private void HandleWings()
        {
            // Basically if you move (as in moveyour player via stick)
            // Wings will get less mobile for convenience
            // Also don't try to tweak these without unity editor, it'll take forever.
            if (NewMovement.Instance.rb.velocity.magnitude > .1f)
            {
                WingBone.material.damping = .2f;
                WingBone.material.stiffness = .65f;
                WingBone.material.resistance = 0;
                WingBone.material.slackness = 0;
            }
            else
            {
                WingBone.material.damping = .2f;
                WingBone.material.stiffness = .1f;
                WingBone.material.resistance = .9f;
                WingBone.material.slackness = .1f;
            }
        }

        private void HandleAnimations()
        {
            // triggers in the air
            if (!NewMovement.Instance.gc.onGround) Anim.SetBool("Jumping", true);
            else Anim.SetBool("Jumping", false);

            // triggers when a player is either sliding or pulling himself towards something heavy (e.g. hookpoint, maurice)
            if (NewMovement.Instance.sliding || (HookArm.Instance?.state == HookState.Pulling && (bool)!HookArm.Instance?.lightTarget))
            { Anim.SetBool("Jumping", false); Anim.SetBool("Sliding", true); }
            else Anim.SetBool("Sliding", false);
        }
        private void HandlePelvisRotation()
        {
            // this exists because my foot placement logic does not want to work *horizontally*
            Vector3 Direction = new Vector3(-Input.InputVars.MoveVector.y, 0, Input.InputVars.MoveVector.x);
            Quaternion Rotation = Quaternion.Euler(0, Quaternion.LookRotation(Direction, Vector3.up).eulerAngles.y - 90, 0);
            if (Input.InputVars.MoveVector.y < 0) Rotation = Quaternion.Euler(0, -Rotation.eulerAngles.y + 90, 0);
            Rig.Pelvis.rotation = Quaternion.Lerp(Rig.Pelvis.rotation, Rotation, Time.deltaTime * 5);
        }
    }
}
