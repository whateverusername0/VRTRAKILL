using UnityEngine;
using Valve.VR;
using Plugin.Util;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    // lol the name
    public class ControllerController : MonoBehaviour
    {
        public GameObject RenderModel;
        public Vector3 RenderModelOffsetPos = Vector3.zero;
        public Vector3 RenderModelOffsetEulerAngles = Vector3.zero;
        private bool? _ShouldRenderRM = null; public bool ShouldRenderRM
        {
            get { return _ShouldRenderRM ?? Vars.Config.Controllers.DrawControllers && Vars.IsMainMenu; }
            set { _ShouldRenderRM = value; }
        }

        private SteamVR_Behaviour_Pose Pose;
        public GameObject GunOffset = new GameObject("Gun Offset") { layer = (int)Layers.IgnoreRaycast };
        public GameObject ArmOffset = new GameObject("Arm Offset") { layer = (int)Layers.IgnoreRaycast };

        GameObject Pointer;
        LineRenderer LR; Vector3 EndPosition;
        public float DefaultLength => Vars.Config.CBS.CrosshairDistance;

        private void SetupOffsets()
        {
            GunOffset.transform.parent = transform;
            GunOffset.transform.localPosition = Vector3.zero;
            GunOffset.transform.localRotation = Quaternion.Euler(45, 0, 0);

            ArmOffset.transform.parent = transform;
        }

        private void SetupControllerLines()
        {
            LR = Pointer.AddComponent<LineRenderer>();
            LR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            LR.receiveShadows = false;
            LR.allowOcclusionWhenDynamic = false;
            LR.useWorldSpace = true;
            LR.material = new Material(Shader.Find("GUI/Text Shader"));

            Color C1 = new Color(1, 1, 1, Vars.Config.UIInteraction.ControllerLines.StartAlpha),
                  C2 = new Color(1, 1, 1, Vars.Config.UIInteraction.ControllerLines.EndAlpha);

            LR.startWidth = 0.02f; LR.endWidth = 0.001f;
            LR.startColor = C1; LR.endColor = C2;
        }

        private void CPRaycast()
        {
            bool Raycast = Physics.Raycast(GunOffset.transform.position, GunOffset.transform.forward,
                                           out RaycastHit Hit, float.PositiveInfinity, (int)Layers.UI);
            EndPosition = GunOffset.transform.position + (GunOffset.transform.forward * DefaultLength);
            if (Raycast) EndPosition = Hit.point;
        }
        private void DrawControllerLines()
        {
            if (LR == null) return;

            if (Vars.IsPlayerFrozen || Vars.IsPlayerUsingShop) LR.enabled = true;
            else LR.enabled = false;

            if (LR.enabled)
            {
                LR.SetPosition(0, GunOffset.transform.position);
                LR.SetPosition(1, EndPosition);
            }
        }

        public void Start()
        {
            RenderModel = RenderModel ?? transform.Find("Model").gameObject;
            Pose = GetComponent<SteamVR_Behaviour_Pose>();

            SetupOffsets();

            if (Vars.Config.UIInteraction.ControllerLines.Enabled && gameObject.HasComponent<GunController>())
                SetupControllerLines();
        }
        public void Update()
        {
            // controller model
            if (ShouldRenderRM)
            {
                RenderModel?.gameObject.SetActive(true);
                RenderModel.transform.localPosition = RenderModelOffsetPos;
                RenderModel.transform.localRotation = Quaternion.Euler(RenderModelOffsetEulerAngles);
            }
            else RenderModel?.gameObject.SetActive(false);

            // paused
            if (Vars.IsPaused && !Vars.IsMainMenu) Pose.enabled = false;
            else Pose.enabled = true;

            // controller-based ui interaction
            if (Vars.Config.UIInteraction.ControllerBased) CPRaycast();
            if (Vars.Config.UIInteraction.ControllerLines.Enabled) DrawControllerLines();
        }

        public static Vector3 ControllerOffset = new Vector3(0, 2.85f, 0);
        public static void onTransformUpdatedH(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
        => fromAction.transform.position += ControllerOffset;
    }
}