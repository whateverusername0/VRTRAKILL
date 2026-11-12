using System.Collections.Generic;
using Valve.VR;

namespace VRTRAKILL.Plugin.Patches.Misc.Tutorials;

internal class ControlMessageData
{
    public string[] Bindings = new string[0];
    public string OriginalText = string.Empty;
    public string Replacement = string.Empty;

    public static string FriendlyBindingName(ISteamVR_Action_In action)
    {
        var direction = string.Empty;
        switch (action.activeDevice)
        {
            case SteamVR_Input_Sources.LeftHand:
            case SteamVR_Input_Sources.LeftFoot:
            case SteamVR_Input_Sources.LeftShoulder:
                direction = "Left"; break;
            case SteamVR_Input_Sources.RightHand:
            case SteamVR_Input_Sources.RightFoot:
            case SteamVR_Input_Sources.RightShoulder:
                direction = "Right"; break;
            default: direction = string.Empty; break;
        }

        if (string.IsNullOrEmpty(direction))
            return action.GetLocalizedOriginPart(SteamVR_Input_Sources.Any);
        else return $"{direction} {action.GetLocalizedOriginPart(SteamVR_Input_Sources.Any)}";
    }

    public string GetConvertedText(string raw)
    {
        if (!raw.Contains(OriginalText))
            return string.Empty;

        if (Bindings.Length == 0)
            return Replacement;
        else return string.Format(Replacement, Bindings);
    }

    public ControlMessageData(string originalText, string replacement)
    {
        OriginalText = originalText;
        Replacement = replacement;
    }

    public ControlMessageData(string originalText, string replacement, params string[] bindings)
        : this(originalText, replacement)
        => Bindings = bindings;

    public ControlMessageData(string originalText, string replacement, params SteamVR_Action_Boolean[] binds)
        :this(originalText, replacement)
    {
        var bindss = new List<string>();
        foreach (var bind in binds)
            bindss.Add(FriendlyBindingName(bind));

        Bindings = bindss.ToArray();
    }
}
