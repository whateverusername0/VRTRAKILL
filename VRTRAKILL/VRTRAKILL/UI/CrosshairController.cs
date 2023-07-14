using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.UI
{
    internal class CrosshairController : MonoBehaviour
    {
        public float DefaultLength => Vars.Config.View.VRUI.CrosshairDistance;

        Vector3 Offset = new Vector3(-.2f, -2.75f, 0);

        public void LateUpdate()
        {
            transform.position = transform.parent.position + (transform.parent.forward * DefaultLength) + Offset;
        }
    }
}
