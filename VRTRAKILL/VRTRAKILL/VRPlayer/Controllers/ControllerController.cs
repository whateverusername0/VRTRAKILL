using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    // lol the name
    internal class ControllerController : MonoBehaviour
    {
        public GameObject Offset = new GameObject("Offset");

        LineRenderer LR;

        PointerEventData PED = new PointerEventData(EventSystem.current);
        List<RaycastResult> RCResults = new List<RaycastResult>();

        private void SetLines()
        {
            Offset.transform.parent = this.transform;
            Offset.transform.localPosition = new Vector3(0, 0, 1);
            Offset.transform.localRotation = Quaternion.Euler(45, 0, 0);

            Color C  = new Color(1, 1, 1, 0.4f),
                  C2 = new Color(1, 1, 1, 0.1f);

            LR.endWidth = 0.001f;
            LR.startWidth = 0.02f;
            LR.startColor = C2;
            LR.endColor = C;

            LR.SetPosition(0, transform.GetChild(0).GetChild(2).position);
            LR.SetPosition(1, Offset.transform.position);
        }

        private void Start()
        {
            LR = gameObject.AddComponent<LineRenderer>();
            LR.material = new Material(Shader.Find("GUI/Text Shader"));
            SetLines();
        }
        private void Update()
        {
            PED.position = Offset.transform.position;
            EventSystem.current.RaycastAll(PED, RCResults);

            if (Vars.IsAMenu || Vars.Config.VRSettings.DrawControllerLines)
            {
                LR.enabled = true;
                SetLines();
            } else LR.enabled = false;
        }

        public static void onTransformUpdatedH(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
        {
            fromAction.transform.position = new Vector3(fromAction.transform.position.x,
                                                        fromAction.transform.position.y + 1.4f,
                                                        fromAction.transform.position.z);
        }
    }
}
