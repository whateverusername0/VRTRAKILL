using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thing : MonoBehaviour
{
    public Transform Origin, Limiter, Offset, Camera;
    private Rigidbody rb;
    private Vector3 OriginalLocalPos;

    private void Start()
    {
        OriginalLocalPos = Limiter.localPosition;
        rb = Offset.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 Position = transform.TransformPoint(OriginalLocalPos);
        rb.position = Position;
        
    }
}
