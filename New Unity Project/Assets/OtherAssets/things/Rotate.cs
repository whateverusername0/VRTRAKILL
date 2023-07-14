using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform Forward;
    public Transform Backward;

    void Update()
    {
        transform.forward = Forward.position.normalized;
        Backward.position = transform.forward * -1;
    }
}
