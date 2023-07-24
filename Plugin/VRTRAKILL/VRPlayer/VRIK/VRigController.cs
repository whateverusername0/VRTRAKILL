using Plugin.VRTRAKILL.VRPlayer.Controllers;
using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    internal class VRigController : MonoBehaviour
    {
        private static VRigController _Instance; public static VRigController Instance { get { return _Instance; } }

        public MetaRig Rig;

        public Vector3 LFBArmScale = new Vector3(0.0125f, 0.0125f, 0.0125f),
                       LArmScale = new Vector3(0.125f, 0.125f, 0.125f),
                       RArmScale = new Vector3(-0.0125f, 0.0125f, 0.0125f);

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
            if (Rig == null) Rig = MetaRig.CreateV1CustomPreset(Vars.VRCameraContainer);
            Helpers.Misc.RecursiveChangeLayer(Rig.GameObjectT.gameObject, (int)Vars.Layers.AlwaysOnTop);

            // transform shenanigans
            Rig.GameObjectT.localPosition = Vector3.zero;
            Rig.GameObjectT.localRotation = Quaternion.Euler(Vector3.zero);

            Rig.Root.localScale *= 3;
            Rig.Body.localPosition = new Vector3(0, -0.0125f, 0.002f);

            // for now don't use these GOs
            Rig.LeftLeg.GameObjecT.localScale = Vector3.zero;
            Rig.RightLeg.GameObjecT.localScale = Vector3.zero;

            // left handed mode support
            if (Vars.Config.Controllers.HandS.LeftHandMode)
            {
                Vector3 TempLAPos = Rig.LFeedbacker.GameObjecT.localPosition,
                        TempRAPos = Rig.RFeedbacker.GameObjecT.localPosition;
                Rig.LFeedbacker.GameObjecT.localPosition = TempRAPos;
                Rig.RFeedbacker.GameObjecT.localPosition = TempLAPos;

                Rig.LFeedbacker.GameObjecT.localScale = new Vector3(Rig.LFeedbacker.GameObjecT.localScale.x * -1,
                                                                    Rig.LFeedbacker.GameObjecT.localScale.y,
                                                                    Rig.LFeedbacker.GameObjecT.localScale.z);
                Rig.RFeedbacker.GameObjecT.localScale = new Vector3(Rig.RFeedbacker.GameObjecT.localScale.x * -1,
                                                                    Rig.RFeedbacker.GameObjecT.localScale.y,
                                                                    Rig.RFeedbacker.GameObjecT.localScale.z);
            }

            // left arm IKs
            AddArmIK(Rig.LFeedbacker.Hand.Root.gameObject,     ArmController.Instance.CC.ArmOffset.transform, Pole: Rig.LFeedbacker.Pole);
            AddArmIK(Rig.LKnuckleblaster.Hand.Root.gameObject, ArmController.Instance.CC.ArmOffset.transform, Pole: Rig.LKnuckleblaster.Pole);
            AddArmIK(Rig.LWhiplash.Hand.Root.gameObject,       ArmController.Instance.CC.ArmOffset.transform, Pole: Rig.LWhiplash.Pole);
            // right arm IKs
            AddArmIK(Rig.RFeedbacker.Hand.Root.gameObject,     GunController.Instance.CC.ArmOffset.transform, Pole: Rig.RFeedbacker.Pole);
            AddArmIK(Rig.RSandboxer.Hand.Root.gameObject,      GunController.Instance.CC.ArmOffset.transform, Pole: Rig.RSandboxer.Pole);

            // leg IKs
            // tbd
        }
        
        public void LateUpdate()
        {
            if (Rig == null) return;

            if (Vars.IsPaused && !Vars.IsAMenu)
            {
                Rig.Head.localScale = Vector3.one;
            }
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
                Rig.LFeedbacker.Hand.Root.localScale = Vector3.one;
                Rig.LKnuckleblaster.Hand.Root.localScale = Vector3.one;
                Rig.LWhiplash.Hand.Root.localScale = Vector3.one;
                Rig.RFeedbacker.Hand.Root.localScale = Vector3.one;
                Rig.RSandboxer.Hand.Root.localScale = Vector3.one;
            }
            else
            {
                Rig.LFeedbacker.Hand.Root.localScale = Vector3.zero;
                Rig.LKnuckleblaster.Hand.Root.localScale = Vector3.zero;
                Rig.LWhiplash.Hand.Root.localScale = Vector3.zero;
                Rig.RFeedbacker.Hand.Root.localScale = Vector3.zero;
                Rig.RSandboxer.Hand.Root.localScale = Vector3.zero;
            }

            // sandbox arm
            foreach (Sandbox.Arm.SandboxArm SA in Resources.FindObjectsOfTypeAll<Sandbox.Arm.SandboxArm>())
                if (SA.enabled && SA.gameObject.activeInHierarchy && SA.currentMode != null)
                {
                    Rig.RFeedbacker.GameObjecT.localScale = Vector3.zero;
                    Rig.RSandboxer.GameObjecT.localScale = RArmScale;
                }
                else
                {
                    Rig.RFeedbacker.GameObjecT.localScale = RArmScale;
                    Rig.RSandboxer.GameObjecT.localScale = Vector3.zero;
                }

            // arm swap
            foreach (Punch P in FindObjectsOfType<Punch>())
                if (P != null && P.enabled && P.gameObject.activeSelf)
                    switch(P.type)
                    {
                        case FistType.Standard:
                            {
                                Rig.LFeedbacker.GameObjecT.localScale = LFBArmScale;
                                Rig.LKnuckleblaster.GameObjecT.localScale = Vector3.zero;
                                break;
                            }
                        case FistType.Heavy:
                            {
                                Rig.LFeedbacker.GameObjecT.localScale = Vector3.zero;
                                Rig.LKnuckleblaster.GameObjecT.localScale = LArmScale;
                                break;
                            }
                        case FistType.Spear:
                        default: break;
                    }
            // whiplash
            if (HookArm.Instance != null && HookArm.Instance.enabled && HookArm.Instance.model.activeSelf)
            {
                Rig.LFeedbacker.GameObjecT.localScale = Vector3.zero;
                Rig.LWhiplash.GameObjecT.localScale = LArmScale;
                Rig.LKnuckleblaster.GameObjecT.localScale = Vector3.zero;
            }
            else Rig.LWhiplash.GameObjecT.localScale = Vector3.zero;
        }
        private void MoveIKPoles()
        {
            Vector3 Position = Vars.DominantHand.transform.forward * -5;
            Rig.LFeedbacker.Pole.position = Position;
            Rig.LKnuckleblaster.Pole.position = Position;
            Rig.LWhiplash.Pole.position = Position;
            Rig.RFeedbacker.Pole.position = Position;
            Rig.RSandboxer.Pole.position = Position;
        }
    }
}
