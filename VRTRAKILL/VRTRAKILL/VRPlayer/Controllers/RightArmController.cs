using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class RightArmController : MonoSingleton<RightArmController>
    {
        public GameObject Offset = new GameObject("Offset");
        LineRenderer lr;
        PointerEventData ped = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();

        public void Start()
        {
            Offset.transform.parent = this.transform;
            Offset.transform.localPosition = new Vector3(0, 0, 1);
            Offset.transform.localRotation = Quaternion.Euler(45, 0, 0);

            lr = gameObject.AddComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("GUI/Text Shader"));
            SetLines();
        }
        void SetLines()
        {
            Color col = new Color(1, 1, 1, 0.1f);
            Color col2 = new Color(1, 1, 1, 0.4f);
            lr.endWidth = 0.001f;
            lr.startWidth = 0.02f;
            lr.startColor = col2;
            lr.endColor = col;
            lr.SetPosition(0, transform.GetChild(0).GetChild(2).position);
            lr.SetPosition(1, Offset.transform.position);
        }

        public void Update()
        {
            ped.position = Offset.transform.position;
            EventSystem.current.RaycastAll(ped, results);
            if (Vars.IsAMenu)
            {
                lr.enabled = true;
                SetLines();
            }
            else
            {
                lr.enabled = false;
            }
        }
    }
}
