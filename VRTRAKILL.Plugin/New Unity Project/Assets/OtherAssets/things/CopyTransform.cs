using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    public Transform Origin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Origin == null) return;
        else
        {
            transform.position = new Vector3(Origin.position.x, transform.position.y, Origin.position.z);
            transform.rotation = Origin.rotation * Quaternion.Euler(-90, 0, 0);
        }
    }
}
