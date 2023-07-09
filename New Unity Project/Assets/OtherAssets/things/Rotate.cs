using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform P1;
    public Transform P2;

    void Update()
    {
        transform.forward = P2.position - P1.position;
    }
}
