using UnityEngine;

namespace Plugin.VRTRAKILL.UI
{
    // "borrowed" from huskvr
    internal sealed class UICanvas : MonoBehaviour
    {
        public Camera Target;

        private Vector3 LastCamFwd = Vector3.zero;

        private const float Distance = 72f;
        private static float Scale => Vars.Config.UIInteraction.UISize;

        private void UpdatePos()
        {
            LastCamFwd = Target.transform.forward * Distance;
            transform.rotation = Target.transform.rotation;
        }
        private void ResetPos()
        {
            LastCamFwd = new Vector3(LastCamFwd.x, 0f, LastCamFwd.z);
            transform.LookAt(Target.transform);
            transform.forward = new Vector3(-transform.forward.x, 0f, -transform.forward.z);
        }

        public void Start()
        {
            transform.localScale = Vector3.one * Scale;
            LastCamFwd = Vector3.back * Distance;
            UpdatePos();
        }
        public void Update()
        {
            if (!Vars.IsPlayerFrozen) UpdatePos(); else ResetPos();
            transform.position = Target.transform.position + LastCamFwd;
        }
    }
}
