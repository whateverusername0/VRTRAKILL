using Valve.VR;

namespace Plugin.VRTRAKILL.Input.ControlMessages
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


        public static string T_Punch
            => $"Hold '<color=orange>{Punch}</color>' and fling your arm to <color=orange>PUNCH</color>";
        public static string T_Slide
            => $"Hold '<color=orange>{Slide}</color>' to <color=orange>SLIDE</color>.";
        public static string T_JumpDash
            => $"Press '<color=orange>{Jump}</color> to <color=orange>JUMP</color>'\r\n" +
               $"Press '<color=#00DFFF>{Dash}</color>' to <color=#00DFFF>DASH</color>. (Consumes <color=#00DFFF>STAMINA</color>)\r\n" +
               $"Can be performed in air.";
        public static string T_Slam
            => $"Press '<color=orange>{Slide}</color>' in the air to <color=orange>GROUND SLAM</color>.\r\n" +
               $"Hold for <color=orange>SHOCKWAVE</color>. (Consumes <color=#00DFFF>STAMINA</color>)";
        public static string T_Prelude_Slam =>
            $"<color=orange>GROUND SLAM</color> deals damage on direct hit.";

        public static string T_Revolver
            => $"<color=cyan>REVOLVER</color>: Hold <color=orange>{Grip}</color> to charge a <color=orange>PIERCING</color> shot.";
        public static string T_Shotgun
            => $"<color=cyan>SHOTGUN</color>: Press '<color=orange>{Grip}</color>' to fire an explosive. Hold to charge distance.";
        public static string T_Nailgun
            => $"<color=cyan>NAILGUN</color>: Use <color=orange>{Grip}</color> to fire a <color=orange>NAIL MAGNET</color>. Can be attached to environment to create traps.";

        public static string T_Knuckleblaster =>
            $"Cycle through <color=orange>EQUIPPED</color> arms with '<color=orange>{SwapArm}</color>'";
        public static string T_Lust_ArmSwapReminder =>
            $"Only the <color=cyan>FEEDBACKER</color> (<color=cyan>Blue arm</color>) can <color=orange>PARRY PROJECTILES</color>.";
        public static string T_Whiplash =>
            $"<color=lime>WHIPLASH</color>: Hold <color=orange>{Whiplash}</color> to throw, release to pull";
    }
}
