using UnityEngine;
using UnityEngine.EventSystems;
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

        // gives out an oob / oor error when not in main menu.
        private void SetLRLines()
        {
            Color C  = new Color(1, 1, 1, Vars.Config.VRSettings.CL.LInitTransparency),
                  C2 = new Color(1, 1, 1, Vars.Config.VRSettings.CL.LEndTransparency);

            LR.endWidth = 0.001f;
            LR.startWidth = 0.02f;
            LR.startColor = C;
            LR.endColor = C2;

            LR.SetPosition(0, transform.GetChild(0).GetChild(2).position);
            LR.SetPosition(1, Offset.transform.position);
        }

        public void Start()
        {
            Offset.transform.parent = this.transform;
            Offset.transform.localPosition = Vector3.zero;
            Offset.transform.localRotation = Quaternion.Euler(45, 0, 0);

            //LR = Offset.AddComponent<LineRenderer>();
            //LR.material = new Material(Shader.Find("GUI/Text Shader"));
            //SetLRLines();
        }
        public void Update()
        {
            PED.position = Offset.transform.position;
            EventSystem.current.RaycastAll(PED, RCResults);

            if (Vars.IsAMenu /*|| Vars.Config.VRSettings.DrawControllerLines*/)
            {
                //LR.enabled = true;
                //SetLRLines();
            } //else LR.enabled = false;
        }

        public static void onTransformUpdatedH(SteamVR_Behaviour_Pose fromAction, SteamVR_Input_Sources fromSource)
        {
            fromAction.transform.position = new Vector3(fromAction.transform.position.x,
                                                        fromAction.transform.position.y + 1.4f,
                                                        fromAction.transform.position.z);
        }
    }
}
