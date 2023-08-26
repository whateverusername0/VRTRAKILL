using UnityEngine;

namespace Plugin.VRTRAKILL.UI
{
    internal class StandardHUDWorker : MonoBehaviour
    {
        public GameObject StandardHUD;

        public void Update()
        {
            if (StandardHUD == null) return;

            if (MonoSingleton<PrefsManager>.Instance.GetInt("hudType") != 1) StandardHUD.SetActive(false);
            else StandardHUD.SetActive(true);
        }
    }
}