namespace Plugin.VRTRAKILL.Config.Input
{
    internal class VRInputSettings
    {
        public float Deadzone { get; set; } = 0.4f;
        public bool SnapTurning { get; set; } = false; // placeholder
        public int SnapTurningAngles { get; set; } = 45; // placeholder
        public int SmoothTurningSpeed { get; set; } = 300;
    }
}
