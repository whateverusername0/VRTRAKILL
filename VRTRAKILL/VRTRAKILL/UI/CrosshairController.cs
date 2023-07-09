using UnityEngine;
using Plugin.Helpers;

namespace Plugin.VRTRAKILL.UI
{
    internal class CrosshairController : MonoBehaviour
    {
        public float DefaultLength => Vars.Config.View.VRUI.CrosshairDistance;

        public void LateUpdate()
        {
            transform.localPosition = new Vector3(-.2f, -2.75f, Vars.Config.View.VRUI.CrosshairDistance);
        }
    }
}
