using UnityEngine;

namespace VRTRAKILL.Plugin.Utilities;

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
        => to.CopyFrom(from); // lol

    public static bool HasComponent<T>(this GameObject GM) where T : Component
        => GM.GetComponent<T>() != null;

    public static T EnsureComponent<T>(this GameObject go) where T : Component
        => !go.TryGetComponent<T>(out var comp) ? go.AddComponent<T>() : comp;
}