using UnityEngine;

namespace Plugin.VRTRAKILL.UI.HUD
{
    internal class HideWhenMenuActive : MonoBehaviour
    {
        public void Update()
        {
            if (Vars.IsAMenu) gameObject.GetComponent<Canvas>().enabled = false;
            else gameObject.GetComponent<Canvas>().enabled = true;
        }
    }
}
