using VRTRAKILL.Data;
using UnityEngine;

namespace VRTRAKILL.Systems.UI;

public class VRHideCanvasOnMenuActive : MonoBehaviour
{
    private bool ShouldHide => Vars.IsPlayerFrozen;

    public void Update()
    {
        if (ShouldHide) gameObject.GetComponent<Canvas>().enabled = false;
        else gameObject.GetComponent<Canvas>().enabled = true;
    }
}
