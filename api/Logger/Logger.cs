namespace Logger
{
    public sealed class Logger<T> : ILogger<T>
    {
        private readonly ILogger<T> _logger;

        public Logger(ILogger<T> logger) => _logger = logger;

        public void Info(string message, params object[] args) =>
            _logger.Info(message, args);

        public void Warn(string message, params object[] args) =>
            _logger.Warn(message, args);

        public void Error(string message, Exception? ex, params object[] args) =>
            _logger.Error(message, ex, args);

        public void Debug(string message, params object[] args) =>
            _logger.Debug(message, args);

    }
}