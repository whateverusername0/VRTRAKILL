namespace VRTRAKILL.UIManager.Model.Logging
{
    public interface ILogger
    {
        string Debug(string Message);
        string Info(string Message);
        string Warn(string Message);
        string Error(string Message);
        string Fatal(string Message);
    }
}
