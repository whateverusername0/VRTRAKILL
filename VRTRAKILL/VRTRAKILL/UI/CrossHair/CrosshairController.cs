using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.UI.CrossHair
{
    internal class CrosshairController : MonoBehaviour
    {
        public float DefaultLength => Vars.Config.View.VRUI.CrosshairDistance;
        Vector3 EndPosition = Vector3.zero;

        public void LateUpdate()
        {
            RaycastHit Hit = transform.parent.ForwardRaycast(DefaultLength);
            EndPosition = transform.parent.position + (transform.parent.forward * DefaultLength);
            if (Hit.collider != null && Hit.transform.gameObject.layer != (int)Vars.Layers.IgnoreRaycast) EndPosition = Hit.point;

            transform.position = EndPosition;
        }
    }
}
