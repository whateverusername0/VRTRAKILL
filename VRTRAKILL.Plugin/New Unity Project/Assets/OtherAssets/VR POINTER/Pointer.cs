using UnityEngine;
using UnityEngine.EventSystems;

public class Pointer : MonoBehaviour
{
    public float DefaultLength = 10;
    public VRInputModule InputModule;

    public LineRenderer LR;

    private void Update()
    {
        UpdateLine();
    }
    private void UpdateLine()
    {
        RaycastHit Hit = CreateRaycast(DefaultLength);

        Vector3 EndPos = transform.position + (transform.forward * DefaultLength);

        if (Hit.collider != null) EndPos = Hit.point;

        LR.SetPosition(0, transform.position);
        LR.SetPosition(1, EndPos);
    }

    private RaycastHit CreateRaycast(float Length)
    {
        Physics.Raycast(new Ray(transform.position, transform.forward), out RaycastHit Hit, DefaultLength);
        return Hit;
    }
}
