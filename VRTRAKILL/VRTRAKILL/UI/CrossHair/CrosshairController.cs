using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.UI.CrossHair
{
    internal class CrosshairController : MonoBehaviour
    {
        public float DefaultLength => Vars.Config.VRSettings.VRUI.CrosshairDistance;
        Vector3 EndPosition = Vector3.zero;

        public void LateUpdate()
        {
            RaycastHit Hit = transform.parent.ForwardRaycast(DefaultLength, (int)Vars.Layers.Default);
            EndPosition = transform.parent.position + (transform.parent.forward * DefaultLength);
            if (Hit.collider != null) EndPosition = Hit.point;

            transform.position = EndPosition;
        }
    }
}
