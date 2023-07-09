using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    internal class VRigController : MonoBehaviour
    {
        private static VRigController _Instance; public static VRigController Instance { get { return _Instance; } }

        public MetaRig Rig;

        public void Awake()
        {
            if (_Instance != null && _Instance != this) Destroy(this.gameObject);
            else _Instance = this;
        }

        public void Start()
        {
            if (Rig == null) Rig = MetaRig.CreateV1CustomPreset(Vars.VRCameraContainer);
            Helpers.Misc.RecursiveChangeLayer(Rig.GameObjectT.gameObject, (int)Vars.Layers.AlwaysOnTop);

            Rig.LFeedbacker.GameObjecT.gameObject.SetActive(true);
            Rig.LKnuckleblaster.GameObjecT.gameObject.SetActive(true);
            Rig.LWhiplash.GameObjecT.gameObject.SetActive(true);
            Rig.RFeedbacker.GameObjecT.gameObject.SetActive(true);
            Rig.RSandboxer.GameObjecT.gameObject.SetActive(true);

            // transform shenanigans
            Rig.GameObjectT.localPosition = Vector3.zero;
            Rig.GameObjectT.localRotation = Quaternion.Euler(Vector3.zero);

            Rig.Root.localScale *= 3;
            Rig.Body.localPosition = new Vector3(0, -.005f, -.0005f);

            // for now don't use these GOs
            Rig.LeftLeg.GameObjecT.localScale = Vector3.zero;
            Rig.RightLeg.GameObjecT.localScale = Vector3.zero;
            Rig.Head.localScale = Vector3.zero;

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

            // add vrik
            IKArm LFIK = Rig.LFeedbacker.Hand.Root.gameObject.AddComponent<IKArm>();
            LFIK.ChainLength = 3; LFIK.Target = Vars.NonDominantHand.transform; LFIK.Pole = Rig.LForearm_Pole;
            IKArm LKIK = Rig.LKnuckleblaster.Hand.Root.gameObject.AddComponent<IKArm>();
            LKIK.ChainLength = 3; LKIK.Target = Vars.NonDominantHand.transform; LKIK.Pole = Rig.LForearm_Pole;
            IKArm LWIK = Rig.LWhiplash.Hand.Root.gameObject.AddComponent<IKArm>();
            LWIK.ChainLength = 3; LWIK.Target = Vars.NonDominantHand.transform; LWIK.Pole = Rig.LForearm_Pole;

            IKArm RFIK = Rig.RFeedbacker.Hand.Root.gameObject.AddComponent<IKArm>();
            RFIK.ChainLength = 3; RFIK.Target = Vars.DominantHand.transform; RFIK.Pole = Rig.RForearm_Pole;
            IKArm RSIK = Rig.RSandboxer.Hand.Root.gameObject.AddComponent<IKArm>();
            RSIK.ChainLength = 3; RSIK.Target = Vars.DominantHand.transform; RSIK.Pole = Rig.RForearm_Pole;
        }
        
        public void LateUpdate()
        {
            if (Rig == null) return;

            HandleBodyRotation();
            HandleHeadRotation();
            HandleArms();
        }

        private void HandleBodyRotation()
        {
            Rig.Root.position = Vars.MainCamera.transform.position;
            // rotate the legger
            Vector3 MoveDirection = new Vector3(Input.InputVars.MoveVector.x, 0, Input.InputVars.MoveVector.y).normalized;
            if (MoveDirection != Vector3.zero)
            {
                Quaternion Rotation = Quaternion.LookRotation(MoveDirection, Vector3.up);
                Rig.Root.rotation = Quaternion.RotateTowards(Rig.Root.rotation, Rotation, 720 * Time.deltaTime);
            }
            // rotate the ABSer
            if ((Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Abdomen.rotation.eulerAngles.y) >= Quaternion.Euler(0, 90, 0).y
            || (Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Abdomen.rotation.eulerAngles.y) <= Quaternion.Euler(0, -90, 0).y)
            {
                Quaternion Rotation = Quaternion.Lerp(Rig.Abdomen.rotation, Vars.MainCamera.transform.rotation, Time.deltaTime * 5);
                Rig.Abdomen.rotation = Quaternion.Euler(0, Rotation.eulerAngles.y, 0);
            }
        }
        private void HandleHeadRotation()
        {
            Rig.Head.position = Vars.MainCamera.transform.position;
            Rig.Head.rotation = Vars.MainCamera.transform.rotation;
        }

        private void HandleArms()
        {
            // main menu
            if (Vars.IsMainMenu)
            {
                Rig.LFeedbacker.Hand.Root.localScale = Vector3.one;
                Rig.RFeedbacker.Hand.Root.localScale = Vector3.one;
            }
            else
            {
                Rig.LFeedbacker.Hand.Root.localScale = Vector3.zero;
                Rig.RFeedbacker.Hand.Root.localScale = Vector3.zero;
            }
            // sandbox arm
            if (Sandbox.Arm.SandboxArm.Instance != null && Sandbox.Arm.SandboxArm.Instance.currentMode != null)
            {
                Rig.RFeedbacker.GameObjecT.localScale = Vector3.zero;
                Rig.RSandboxer.GameObjecT.localScale = Vector3.one;
            }
            else
            {
                Rig.RFeedbacker.GameObjecT.localScale = Vector3.one;
                Rig.RSandboxer.GameObjecT.localScale = Vector3.zero;
            }
            // gun
            foreach (Revolver R in FindObjectsOfType<Revolver>())
                if (R != null && R.enabled && R.gameObject.activeSelf) Rig.RFeedbacker.Hand.Root.localScale = Vector3.zero;
                else Rig.RFeedbacker.Hand.Root.localScale = Vector3.one;

            // arm swap
            foreach (Punch P in FindObjectsOfType<Punch>())
                if (P != null && P.enabled && P.gameObject.activeSelf)
                    switch(P.type)
                    {
                        case FistType.Standard:
                            {
                                Rig.LFeedbacker.GameObjecT.localScale = Vector3.one;
                                Rig.LKnuckleblaster.GameObjecT.localScale = Vector3.zero;
                                break;
                            }
                        case FistType.Heavy:
                            {
                                Rig.LFeedbacker.GameObjecT.localScale = Vector3.zero;
                                Rig.LKnuckleblaster.GameObjecT.localScale = Vector3.one;
                                break;
                            }
                        case FistType.Spear:
                        default: break;
                    }
            if (HookArm.Instance != null && HookArm.Instance.enabled && HookArm.Instance.model.activeSelf)
                Rig.LWhiplash.GameObjecT.localScale = Vector3.one;
            else Rig.LWhiplash.GameObjecT.localScale = Vector3.zero;
        }
    }
}
