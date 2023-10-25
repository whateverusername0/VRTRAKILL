using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWalk : MonoBehaviour
{
    public Plugin.VRTRAKILL.VRPlayer.VRIK.IKArm IKArm;
    public Transform Target, Origin;

    private void Start()
    {
        IKArm = GetComponent<Plugin.VRTRAKILL.VRPlayer.VRIK.IKArm>();
        IKArm.enabled = false;
    }
    private Vector3 PrevPos = default;
    public Vector3 Diff;
    private void LateUpdate()
    {
        transform.position = Target.position; transform.rotation = Target.rotation;
        Vector3 InitPos = transform.position; Quaternion InitRot = transform.rotation;

        Diff = Target.position - PrevPos;
        if (Mathf.Abs(Diff.y) > .2f)
        {
            if (Diff.y > 0) Diff.y = .2f;
            else Diff.y = -.2f;
        }

        if (Physics.Raycast(new Ray(Target.parent.position, Vector3.down),
                            out RaycastHit Info, 10, 1 << 0))
        {
            Debug.DrawLine(Target.position, Info.point, Color.black, .1f);
            if (Target.position.y <= Info.point.y)
            {
                Target.position = new Vector3(Target.position.x, Target.position.y - Diff.y, Target.position.z);
            }
            else { PrevPos = Target.position; }
        }

        InitPos = transform.position; InitRot = transform.rotation;

        IKArm.Update();

        transform.position = InitPos; transform.rotation = InitRot;
    }
}
