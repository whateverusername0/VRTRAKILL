namespace VRTRAKILL.UIManager.Model.Logging
{
    internal class VSDebuggerLogger : StringLogger
    {
        public new void Debug(string Message)
        => System.Diagnostics.Debug.WriteLine(base.Debug(Message));
        public new void Info(string Message)
        => System.Diagnostics.Debug.WriteLine(base.Info(Message));
        public new void Warn(string Message)
        => System.Diagnostics.Debug.WriteLine(base.Warn(Message));
        public new void Error(string Message)
        => System.Diagnostics.Debug.WriteLine(base.Error(Message));
        public new void Fatal(string Message)
        => System.Diagnostics.Debug.WriteLine(base.Fatal(Message));
    }
}
