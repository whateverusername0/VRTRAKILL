using HarmonyLib;
using VRTRAKILL.Systems;

namespace VRTRAKILL.Patches.ULTRAKILL.Weapons.Guns;

[HarmonyPatch(typeof(Revolver))] internal static class PatchRevolver
{
    [HarmonyPostfix] [HarmonyPatch(nameof(Revolver.Start))]
    static void Transform(Revolver __instance)
    {
        __instance.wpos.enabled = false;
        if (__instance.altVersion) WeaponTransformHelper.ApplyTransform(ref __instance.wpos, new(.05f, -.075f, .5f), new(), new(.085f, .085f, .085f));
        else WeaponTransformHelper.ApplyTransform(ref __instance.wpos, new(.05f, -.1f, .6f), new(), new(.1f, .1f, .1f));
    }

    // TODO
}