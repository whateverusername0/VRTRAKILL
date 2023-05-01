using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Plugin.VRTRAKILL.VRPlayer.Controllers
{
    internal class LeftArmController : MonoSingleton<LeftArmController>
    {
        public GameObject Offset = new GameObject("Offset");
        private Vector3 _PreviousPosition;
        private Vector3 _CurrentVelocity;
        public float Speed = 0;
        LineRenderer lr;
        PointerEventData ped = new PointerEventData(EventSystem.current);
        List<RaycastResult> results = new List<RaycastResult>();
        private IEnumerator CalculateVelocity()
        {
            _PreviousPosition = transform.position;

            yield return new WaitForEndOfFrame();

            _CurrentVelocity = (_PreviousPosition - transform.position) / Time.deltaTime;
            Speed = _CurrentVelocity.magnitude;
        }

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
            StartCoroutine(CalculateVelocity());
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
