using System;

namespace VRTRAKILL.UIManager.Model.Logging
{
    /// <summary>
    /// All methods return a string that you might use for other loggers or whatever.
    /// See <see cref="ConsoleLogger"/> for usage example.
    /// </summary>
    internal abstract class StringLogger : ILogger
    {
        protected string FriendlyDateTime(DateTime DT)
            => $"[{DT.Hour}:{DT.Minute}:{DT.Second}]";

        public virtual string Debug(string Message)
            => $"(DEBUG) {FriendlyDateTime(DateTime.Now)}: {Message}";
        public virtual string Info(string Message)
            => $"(INFO) {FriendlyDateTime(DateTime.Now)}: {Message}";
        public virtual string Warn(string Message)
            => $"(WARN) {FriendlyDateTime(DateTime.Now)}: {Message}";
        public virtual string Error(string Message)
            => $"(ERROR) {FriendlyDateTime(DateTime.Now)}: {Message}";
        public virtual string Fatal(string Message)
            => $"(FATAL) {FriendlyDateTime(DateTime.Now)}: {Message}";
    }
}
