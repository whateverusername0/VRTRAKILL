using Valve.VR;
using System.Collections.Generic;

namespace VRBasePlugin.ULTRAKILL.Input.ControlMessages
{
    internal class MessageContainer
    {
        private static string Punch => SteamVR_Actions._default.Punch.FriendlyBindingName();
        private static string Slide => SteamVR_Actions._default.Slide.FriendlyBindingName();
        private static string Jump => SteamVR_Actions._default.Jump.FriendlyBindingName();
        private static string Dash => SteamVR_Actions._default.Dash.FriendlyBindingName();
        private static string Grip => SteamVR_Actions._default.AltShoot.FriendlyBindingName();
        private static string SwapArm => SteamVR_Actions._default.SwapHand.FriendlyBindingName();
        private static string Whiplash => SteamVR_Actions._default.Whiplash.FriendlyBindingName();


        private static string Tutorial_Punch
            => $"Hold '<color=orange>{Punch}</color>' and fling your arm to <color=orange>PUNCH</color>";
        private static string Tutorial_Slide
            => $"Hold '<color=orange>{Slide}</color>' to <color=orange>SLIDE</color>.";
        private static string Tutorial_JumpDash
            => $"Press '<color=orange>{Jump}</color>' to <color=orange>JUMP</color>\r\n" +
               $"Press '<color=#00DFFF>{Dash}</color>' to <color=#00DFFF>DASH</color>. (Consumes <color=#00DFFF>STAMINA</color>)\r\n" +
               $"Can be performed in air.";
        private static string Tutorial_Slam
            => $"Press '<color=orange>{Slide}</color>' in the air to <color=orange>GROUND SLAM</color>.\r\n" +
               $"Hold for <color=orange>SHOCKWAVE</color>. (Consumes <color=#00DFFF>STAMINA</color>)";
        private static string Tutorial_PreludeSlam =>
            $"<color=orange>GROUND SLAM</color> deals damage on direct hit.";

        private static string Tutorial_Revolver
            => $"<color=cyan>REVOLVER</color>: Hold <color=orange>{Grip}</color> to charge a <color=orange>PIERCING</color> shot.";
        private static string Tutorial_Shotgun
            => $"<color=cyan>SHOTGUN</color>: Press '<color=orange>{Grip}</color>' to fire an explosive. Hold to charge distance.";
        private static string Tutorial_Nailgun
            => $"<color=cyan>NAILGUN</color>: Use <color=orange>{Grip}</color> to fire a <color=orange>NAIL MAGNET</color>. Can be attached to environment to create traps.";

        private static string TutorialKnuckleblaster =>
            $"Cycle through <color=orange>EQUIPPED</color> arms with '<color=orange>{SwapArm}</color>'";
        private static string Tutorial_Lust_ArmSwapReminder =>
            $"Only the <color=cyan>FEEDBACKER</color> (<color=cyan>Blue arm</color>) can <color=orange>PARRY PROJECTILES</color>.";
        private static string Tutorial_Whiplash =>
            $"<color=lime>WHIPLASH</color>: Hold <color=orange>{Whiplash}</color> to throw, release to pull";

        private static string Book_Close =>
            $"<color=orange>Punch</color> to CLOSE";

        public static readonly Dictionary<string, string> HintsToTexts = new Dictionary<string, string>
        {
            { "PUNCH", Tutorial_Punch },
            { "SLIDE", Tutorial_Slide },
            { "DASH", Tutorial_JumpDash },
            { "SHOCKWAVE", Tutorial_Slam },
            { "deals damage on direct hit.", Tutorial_PreludeSlam },

            { "REVOLVER", Tutorial_Revolver },
            { "SHOTGUN", Tutorial_Shotgun },
            { "NAILGUN", Tutorial_Nailgun },

            { "arms with", TutorialKnuckleblaster },
            { "Only the", Tutorial_Lust_ArmSwapReminder },
            { "to throw, release to pull", Tutorial_Whiplash },

            { "to CLOSE", Book_Close },
        };
    }
}
