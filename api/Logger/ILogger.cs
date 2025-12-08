namespace Logger
{
    public interface ILogger<T>
    {
        void Info(string message, params object[] args);
        void Warn(string message, params object[] args);
        void Error(string message, Exception? exception = null, params object[] args);
        void Debug(string message, params object[] args);
    }
}