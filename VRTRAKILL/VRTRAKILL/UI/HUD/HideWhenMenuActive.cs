using UnityEngine;

namespace Plugin.VRTRAKILL.UI.HUD
{
    internal class HideWhenMenuActive : MonoBehaviour
    {
        public void Update()
        {
            if (Vars.IsAMenu) gameObject.SetActive(false);
            else if (!Vars.IsAMenu && !gameObject.activeSelf) gameObject.SetActive(true);
        }
    }
}
