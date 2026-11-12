using UnityEngine;
using UnityEngine.EventSystems;

namespace VRTRAKILL.Plugin.Input;

internal sealed class NoMouseInput : BaseInput
{
    // Reports that no mouse is present and no mouse inputs are done.
    public override bool mousePresent => false;
    public override bool GetMouseButtonDown(int button) => false;
    public override bool GetMouseButtonUp(int button) => false;
    public override bool GetMouseButton(int button) => false;
    public override Vector2 mouseScrollDelta => Vector2.zero;
}
