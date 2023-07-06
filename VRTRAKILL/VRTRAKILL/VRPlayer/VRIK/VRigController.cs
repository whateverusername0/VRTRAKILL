using UnityEngine;
using static Valve.VR.SteamVR_PlayArea;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    internal class VRigController : MonoBehaviour
    {
        private static VRigController _Instance; public static VRigController Instance { get { return _Instance; } }

        public MetaRig Rig;

        private Vector3 LastNDArmScale, LastDArmScale, LastDHandScale;

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
            Rig.Body.localPosition = new Vector3(0, -.005f, -.0005f);

            // remember last GOs scales
            LastNDArmScale = Rig.LeftArm.GameObjecT.localScale;
            LastDArmScale = Rig.RightArm.GameObjecT.localScale;
            LastDHandScale = Rig.RightArm.Hand.localScale;

            // for now don't use these GOs
            Rig.LeftLeg.GameObjectT.localScale = Vector3.zero;
            Rig.RightLeg.GameObjectT.localScale = Vector3.zero;
            Rig.Head.GetChild(0).transform.localScale = Vector3.zero;

            // left handed mode support
            if (Vars.Config.Controllers.HandS.LeftHandMode)
            {
                Vector3 TempLAPos = Rig.LeftArm.GameObjecT.localPosition,
                        TempRAPos = Rig.RightArm.GameObjecT.localPosition;
                Rig.LeftArm.GameObjecT.localPosition = TempRAPos;
                Rig.RightArm.GameObjecT.localPosition = TempLAPos;

                Rig.LeftArm.GameObjecT.localScale = new Vector3(Rig.LeftArm.GameObjecT.localScale.x * -1,
                                                                 Rig.LeftArm.GameObjecT.localScale.y,
                                                                 Rig.LeftArm.GameObjecT.localScale.z);
                Rig.RightArm.GameObjecT.localScale = new Vector3(Rig.RightArm.GameObjecT.localScale.x * -1,
                                                                  Rig.RightArm.GameObjecT.localScale.y,
                                                                  Rig.RightArm.GameObjecT.localScale.z);
            }

            // add vrik
            IKArm LIKArm = Rig.LeftArm.Hand.gameObject.AddComponent<IKArm>();
            LIKArm.ChainLength = 3; LIKArm.Target = Vars.NonDominantHand.transform;
            IKArm RIKArm = Rig.RightArm.Hand.gameObject.AddComponent<IKArm>();
            RIKArm.ChainLength = 3; RIKArm.Target = Vars.DominantHand.transform;
        }
        
        public void LateUpdate()
        {
            if (Rig == null) return;

            HandleBodyRotation();
            HandleHeadRotation();
            ScaleMRArmsIfNecessary();
        }

        private void HandleBodyRotation()
        {
            if ((Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Root.rotation.eulerAngles.y) >= Quaternion.Euler(0, 90, 0).y
            || (Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Root.rotation.eulerAngles.y) <= Quaternion.Euler(0, -90, 0).y)
            {
                Quaternion Rotation = Quaternion.Lerp(Rig.Root.rotation, Vars.MainCamera.transform.rotation, Time.deltaTime * 5);
                Rig.Root.rotation = Quaternion.Euler(0, Rotation.eulerAngles.y, 0);
            }
            Rig.Root.position = Vars.MainCamera.transform.position;
        }
        private void HandleHeadRotation()
        {
            Rig.Head.position = Vars.MainCamera.transform.position;
            Rig.Head.rotation = Vars.MainCamera.transform.rotation;
        }
        // arm stuffs
        private void ScaleMRArmsIfNecessary()
        {
            if (Vars.IsMainMenu) Rig.LeftArm.GameObjecT.localScale = LastNDArmScale;
            else Rig.LeftArm.GameObjecT.localScale = Vector3.zero;

            if (Sandbox.Arm.SandboxArm.Instance != null && Sandbox.Arm.SandboxArm.Instance.isActiveAndEnabled)
                Rig.RightArm.GameObjecT.localScale = Vector3.zero;
            else Rig.RightArm.GameObjecT.localScale = LastDArmScale;

            foreach (Revolver R in FindObjectsOfType<Revolver>())
                if (R.isActiveAndEnabled && R.gameObject.activeInHierarchy) Rig.RightArm.Hand.localScale = Vector3.zero;
                else Rig.RightArm.Hand.localScale = LastDHandScale;
        }
    }
}
