using HarmonyLib;
using System.Collections.Generic;
using Valve.VR;

namespace VRTRAKILL.Plugin.Patches.Misc.Tutorials;

[HarmonyPatch]
internal static class PatchControlMessages
{
    private static List<ControlMessageData> _overrides = new()
    {
        new("PUNCH",
            "Do a punching motion while holding '<color=cyan>{0}</color>' to PUNCH.\r\n" +
            "Required velocity can be changed or set to 0 if you don't like hitting your walls on accident",
            SteamVR_Actions._default.Punch),

        new("SLIDE",
            "Hold '<color=cyan>{0}</color>' to <color=orange>SLIDE</color>.",
            SteamVR_Actions._default.Slide),

        new("DASH",
            "Press '<color=cyan>{0}</color>' to <color=orange>JUMP</color>\r\n" +
            "Press '<color=cyan>{1}</color>' to <color=orange>DASH</color>.\r\n" +
            "Dashing consumes <color=cyan>STAMINA</color>\r\n" +
            "Can be performed in air.",
            SteamVR_Actions._default.Jump,
            SteamVR_Actions._default.Dodge),

        new("SHOCKWAVE",
            "Press '<color=cyan>{0}</color>' in the air to <color=orange>GROUND SLAM</color>.\r\n" +
            "Hold for <color=orange>SHOCKWAVE</color>. (Consumes <color=cyan>STAMINA</color>)",
            SteamVR_Actions._default.Slide),

        new("deals damage on direct hit.",
            "<color=orange>GROUND SLAM</color> deals damage on direct hit.",
            SteamVR_Actions._default.Slide),

        new("REVOLVER",
            "<color=cyan>REVOLVER</color>\r\n" +
            "Hold <color=cyan>{0}</color> to charge a <color=orange>PIERCING</color> shot.",
            SteamVR_Actions._default.SecondaryFire),

        new("SHOTGUN",
            "<color=cyan>SHOTGUN</color>\r\n" +
            "Press '<color=cyan>{0}</color>' to fire an explosive. Hold to charge distance.",
            SteamVR_Actions._default.SecondaryFire),

        new("NAILGUN",
            "<color=cyan>NAILGUN</color>\r\n" +
            "Use <color=cyan>{0}</color> to fire a <color=orange>NAIL MAGNET</color>.\r\n" +
            "Can be attached to environment to create traps.",
            SteamVR_Actions._default.SecondaryFire),

        new("arms with",
            "Cycle through <color=orange>EQUIPPED</color> arms with '<color=cyan>{0}</color>'",
            SteamVR_Actions._default.ChangeFist),

        new("Only the",
            "Only the <color=cyan>FEEDBACKER</color> can <color=orange>PARRY PROJECTILES</color>.",
            SteamVR_Actions._default.ChangeFist),

        new("to throw, release to pull",
            "<color=lime>WHIPLASH</color>\r\n" +
            "Hold <color=cyan>{0}</color> to throw, release to pull.\r\n" +
            "You can change the direction of the throw in your VRTRAKILL config.",
            SteamVR_Actions._default.Hook),

        new("to CLOSE",
            "<color=orange>Punch</color> to CLOSE",
            SteamVR_Actions._default.Punch),
    };

    [HarmonyPrefix] [HarmonyPatch(typeof(HudMessage), nameof(HudMessage.PlayMessage))]
    private static void PlayMessage(HudMessage __instance, bool hasToBeEnabled = false)
        => __instance.ChangeMessage(ConvertText(ref __instance.message, ref __instance.message2));

    [HarmonyPostfix] [HarmonyPatch(typeof(TextBinds), nameof(TextBinds.OnEnable))]
    private static void TBOnEnable(TextBinds __instance)
        => __instance.text.text = ConvertText(ref __instance.text1, ref __instance.text2);

    private static string ConvertText(ref string msg, ref string msg2)
    {
        var full = msg + msg2;

        if (string.IsNullOrWhiteSpace(full))
            return full;

        foreach (var @override in _overrides)
        {
            var text = @override.GetConvertedText(full);
            if (string.IsNullOrEmpty(text)) continue;

            return text;
        }

        return full;
    }
}
