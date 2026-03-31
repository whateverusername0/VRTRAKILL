using VRTRAKILL.Data;
using UnityEngine;
using Valve.VR;
using VRTRAKILL.Utilities;

namespace VRTRAKILL.Systems.Controllers
{
    // lol the name
    public class VRControllersSystem : MonoBehaviour
    {
        public Transform RenderModel;
        public Vector3 RenderModelOffsetPos,
                       RenderModelOffsetEulerAngles,
                       RenderModelOffsetScale;

        public Transform GunOffset = new GameObject("Gun Offset") { layer = (int)Layers.IgnoreRaycast }.transform;
        public Transform ArmOffset = new GameObject("Arm Offset") { layer = (int)Layers.IgnoreRaycast }.transform;

        LineRenderer LR; Vector3 EndPosition;
        public float DefaultLength => GlobalVars.Config.Controllers.CrosshairDistance;

        private void SetupOffsets()
        {
            GunOffset.parent = transform;
            GunOffset.localPosition = Vector3.zero;
            GunOffset.localRotation = Quaternion.Euler(45, 0, 0);

            ArmOffset.parent = transform;
        }

        private void SetupControllerLines()
        {
            LR = gameObject.AddComponent<LineRenderer>();
            LR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            LR.receiveShadows = false;
            LR.allowOcclusionWhenDynamic = false;
            LR.useWorldSpace = true;
            LR.material = new Material(Shader.Find("GUI/Text Shader"));

            Color C1 = new Color(1, 1, 1, .4f),
                  C2 = new Color(1, 1, 1, .1f);

            LR.startWidth = 0.02f; LR.endWidth = 0.001f;
            LR.startColor = C1; LR.endColor = C2;
        }

        private void CPRaycast()
        {
            bool Raycast = Physics.Raycast(GunOffset.position, GunOffset.forward,
                                           out RaycastHit Hit, float.PositiveInfinity, (int)Layers.UI);
            EndPosition = GunOffset.position + (GunOffset.forward * DefaultLength);
            if (Raycast) EndPosition = Hit.point;
        }
        private void DrawControllerLines()
        {
            if (LR == null) return;

            if (GlobalVars.IsPlayerFrozen || GlobalVars.IsPlayerUsingShop) LR.enabled = true;
            else LR.enabled = false;

            if (LR.enabled)
            {
                LR.SetPosition(0, GunOffset.position);
                LR.SetPosition(1, EndPosition);
            }
        }

        public void Start()
        {
            RenderModel = RenderModel ?? transform.Find("Model");

            SetupOffsets();

            if (gameObject.HasComponent<VRArmsSystem>())
                SetupControllerLines();
        }
        public void Update()
        {
            // controller-based ui interaction
            CPRaycast();
            DrawControllerLines();

            // controller model
            if (GlobalVars.Config.Controllers.DrawControllers)
            {
                RenderModel.localPosition = RenderModelOffsetPos;
                RenderModel.localRotation = Quaternion.Euler(RenderModelOffsetEulerAngles);
                RenderModel.localScale = RenderModelOffsetScale;
            }
        }

        public static Vector3 ControllerOffset = new Vector3(0, 2.85f, 0);
        public static void onTransformUpdatedH(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
        => fromAction.transform.position += ControllerOffset;
    }
}