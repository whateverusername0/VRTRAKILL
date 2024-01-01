using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRArmIK;

public class AvatarPlatformOffset : MonoBehaviour
{
    public bool correctVrPlatformOffsetOnStart = true;

    void Start()
    {
        if (correctVrPlatformOffsetOnStart)
        {
            var p = transform.position;
            p.y -= PoseManager.Instance.vrSystemOffsetHeight;
            transform.position = p;
        }
    }
}
