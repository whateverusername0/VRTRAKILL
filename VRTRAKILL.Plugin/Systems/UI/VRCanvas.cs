using VRTRAKILL.Data;
using UnityEngine;

namespace VRTRAKILL.Systems.UI;

// "borrowed" from huskvr
public class VRCanvas : MonoBehaviour
{
    private Vector3 LastCamFwd = Vector3.zero;

    private const float Distance = 72f;
    private static float Scale => GlobalVars.Config.UISize;

    private void UpdatePos()
    {
        LastCamFwd = VRCanvasHelper.UICamera.transform.forward * Distance;
        transform.rotation = VRCanvasHelper.UICamera.transform.rotation;
    }
    private void ResetPos()
    {
        LastCamFwd = new Vector3(LastCamFwd.x, 0f, LastCamFwd.z);
        transform.LookAt(VRCanvasHelper.UICamera.transform);
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
        if (!GlobalVars.IsPlayerFrozen) UpdatePos(); else ResetPos();
        transform.position = VRCanvasHelper.UICamera.transform.position + LastCamFwd;
    }
}
