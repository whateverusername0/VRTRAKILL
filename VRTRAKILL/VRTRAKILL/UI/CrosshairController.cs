using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.UI
{
    internal class CrosshairController : MonoBehaviour
    {
        public float DefaultLength => Vars.Config.View.VRUI.CrosshairDistance;
        Vector3 EndPosition = Vector3.zero;

        public void LateUpdate()
        {
            EndPosition = transform.parent.position + (transform.parent.forward * DefaultLength);
            transform.position = EndPosition;
        }
    }
}
