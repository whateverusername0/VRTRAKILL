using UnityEngine;

namespace Plugin.VRTRAKILL.VRPlayer.VRIK
{
    internal class VRigController : MonoBehaviour
    {
        private static VRigController _Instance; public static VRigController Instance { get { return _Instance; } }

        public MetaRig Rig;

        private Vector3 LastLArmScale, LastRArmScale, LastRHandScale;

        public void Awake()
        {
            if (_Instance != null && _Instance != this) Destroy(this.gameObject);
            else _Instance = this;
        }

        public void Start()
        {
            if (Rig == null) Rig = MetaRig.CreateV1CustomPreset(Vars.VRCameraContainer);

            // basic setup
            Helpers.Misc.RecursiveChangeLayer(Rig.GameObjectT.gameObject, (int)Vars.Layers.AlwaysOnTop);

            Rig.GameObjectT.localPosition = Vector3.zero;
            Rig.GameObjectT.localRotation = Quaternion.Euler(Vector3.zero);

            Rig.Root.localScale *= 3;
            Rig.Body.localPosition = new Vector3(0, -.005f, -.0005f);

            LastLArmScale = Rig.LeftArm.GameObjectT.localScale;
            LastRArmScale = Rig.RightArm.GameObjectT.localScale;
            LastRHandScale = Rig.RightArm.Hand.localScale;

            IKArm LIKArm = Rig.LeftArm.Hand.gameObject.AddComponent<IKArm>();
            LIKArm.ChainLength = 3; LIKArm.Target = Vars.NonDominantHand.transform;
            IKArm RIKArm = Rig.RightArm.Hand.gameObject.AddComponent<IKArm>();
            RIKArm.ChainLength = 3; RIKArm.Target = Vars.DominantHand.transform;

            Rig.LeftArm.GameObjectT.localScale = Vector3.zero;
            Rig.LeftLeg.GameObjectT.localScale = Vector3.zero;
            Rig.RightLeg.GameObjectT.localScale = Vector3.zero;
            Rig.Head.GetChild(0).transform.localScale = Vector3.zero;
        }
        
        public void LateUpdate()
        {
            if (Rig == null) return;

            HandleBodyRotation();
            ScaleMRArmsIfNecessary();
        }

        private void HandleBodyRotation()
        {
            if ((Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Root.rotation.eulerAngles.y) >= Quaternion.Euler(0, 90, 0).y
            || (Vars.MainCamera.transform.rotation.eulerAngles.y - Rig.Root.rotation.eulerAngles.y) <= Quaternion.Euler(0, -90, 0).y)
            {
                Quaternion Rotation = Quaternion.Lerp(Rig.Root.rotation, Vars.MainCamera.transform.rotation, Time.deltaTime * 7.5f);
                Rig.Root.rotation = Quaternion.Euler(0, Rotation.eulerAngles.y, 0);
            }
            Rig.Root.position = Vars.MainCamera.transform.position;
        }
        // arm stuffs
        private void ScaleMRArmsIfNecessary()
        {
            if (Vars.IsMainMenu) Rig.LeftArm.GameObjectT.localScale = LastLArmScale;
            else Rig.LeftArm.GameObjectT.localScale = Vector3.zero;

            if (Sandbox.Arm.SandboxArm.Instance != null && Sandbox.Arm.SandboxArm.Instance.isActiveAndEnabled)
                Rig.RightArm.GameObjectT.localScale = Vector3.zero;
            else Rig.RightArm.GameObjectT.localScale = LastRArmScale;

            foreach (Revolver R in FindObjectsOfType<Revolver>())
                if (R.isActiveAndEnabled && R.gameObject.activeInHierarchy) Rig.RightArm.Hand.localScale = Vector3.zero;
                else Rig.RightArm.Hand.localScale = LastRHandScale;
        }
    }
}
