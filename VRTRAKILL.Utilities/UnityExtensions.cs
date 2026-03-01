using UnityEngine;

namespace VRTRAKILL.Utilities;

public static class UnityExtensions
{
    public static GameObject ForceFindGameObject(string Name)
    {
        foreach (GameObject GO in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            if (GO.name == Name) return GO;
        return null;
    }

    public static void RecursiveChangeLayer(this GameObject GM, int Layer)
    {
        GM.layer = Layer;
        if (GM.transform.childCount > 0)
            for (int i = 0; i < GM.transform.childCount; i++)
                try { RecursiveChangeLayer(GM.transform.GetChild(i).gameObject, Layer); } catch {}
    }

    public static RaycastHit ForwardRaycast(this Transform T, float Length, int? Layer = null)
    {
        if (Layer != null) { Physics.Raycast(new Ray(T.position, T.forward), out RaycastHit Hit, Length, (int)Layer); return Hit; }
        else { Physics.Raycast(new Ray(T.position, T.forward), out RaycastHit Hit, Length); return Hit; }
    }

    public static void EnableOffscreenRendering(Transform T = null, SkinnedMeshRenderer SMR = null)
    {
        if (T) foreach (SkinnedMeshRenderer _SMR in T.GetComponentsInChildren<SkinnedMeshRenderer>()) _SMR.updateWhenOffscreen = true;
        else if (SMR) SMR.updateWhenOffscreen = true;
        else foreach (SkinnedMeshRenderer _SMR in Object.FindObjectsOfType<SkinnedMeshRenderer>()) _SMR.updateWhenOffscreen = true;
    }

    public static void CopyCameraValues(this Camera from, Camera to)
    {
        to.nearClipPlane = from.nearClipPlane;
        to.farClipPlane = from.farClipPlane;
        to.depth = from.depth;
        to.stereoTargetEye = from.stereoTargetEye;
        to.backgroundColor = from.backgroundColor;
        to.cullingMask = from.cullingMask;
        to.clearFlags = from.clearFlags;
        to.fieldOfView = from.fieldOfView;
    }

    public static bool HasComponent<T>(this GameObject GM) where T : Component
    { return GM.GetComponent<T>() != null; }

    public static T EnsureComponent<T>(this GameObject @gameOject) where T : Component
        => !@gameOject.TryGetComponent<T>(out var comp) ? gameOject.AddComponent<T>() : comp;
}