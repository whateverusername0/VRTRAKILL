using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class EZSoftBoneColliderBase : MonoBehaviour
{
    public static HashSet<EZSoftBoneColliderBase> EnabledColliders = new HashSet<EZSoftBoneColliderBase>();

    protected void OnEnable()
    {
        EnabledColliders.Add(this);
    }
    protected void OnDisable()
    {
        EnabledColliders.Remove(this);
    }

    public abstract void Collide(ref Vector3 position, float spacing);
}
