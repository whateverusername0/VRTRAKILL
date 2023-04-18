using HarmonyLib;
using UnityEngine;
using ULTRAKILL.Cheats;

namespace Plugin.VRTRAKILL.VRPlayer.Movement.Patches
{
    [HarmonyPatch] internal class FlightNoclipP
    {
        [HarmonyPrefix] [HarmonyPatch(typeof(Flight), nameof(Flight.Update))] static bool FlightUpdate(Flight __instance)
        {
            float d = 1f;
            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.IsPressed) d = 2.5f;

            Vector3 a = Vector3.zero;
            Vector2 vector = Vector2.ClampMagnitude(Input.VRInputVars.MoveVector, 1f);

            a += __instance.camera.right * vector.x;
            a += __instance.camera.forward * vector.y;

            if (MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed) a += Vector3.up;
            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.IsPressed) a += Vector3.down;

            __instance.rigidbody.velocity = a * 30f * d;
            MonoSingleton<NewMovement>.Instance.enabled = false;

            __instance.rigidbody.isKinematic = false;
            __instance.rigidbody.useGravity = false;

            return false;
        }

        [HarmonyPrefix][HarmonyPatch(typeof(Noclip), nameof(Noclip.Update))] static bool NoclipUpdate(Noclip __instance)
        {
            float d = 1f;
            if (MonoSingleton<InputManager>.Instance.InputSource.Dodge.IsPressed) d = 2.5f;
            Vector2 vector = Vector2.ClampMagnitude(Input.VRInputVars.MoveVector, 1f);
            __instance.transform.position = __instance.transform.position + __instance.camera.right * vector.x * 40f * Time.deltaTime * d;
            __instance.transform.position = __instance.transform.position + __instance.camera.forward * vector.y * 40f * Time.deltaTime * d;

            if (MonoSingleton<InputManager>.Instance.InputSource.Jump.IsPressed)
                __instance.transform.position = __instance.transform.position + new Vector3(0f, 40f, 0f) * 1f * Time.deltaTime * d;

            if (MonoSingleton<InputManager>.Instance.InputSource.Slide.IsPressed)
                __instance.transform.position = __instance.transform.position + new Vector3(0f, -40f, 0f) * 1f * Time.deltaTime * d;

            MonoSingleton<NewMovement>.Instance.enabled = false;
            __instance.rigidbody.isKinematic = true;

            return false;
        }
    }
}

