namespace Logger
{
    public sealed class ConsoleLogger<T> : ICustomLogger<T>
    {
        private readonly string _categoryName;
        public ConsoleLogger()
        {
            _categoryName = typeof(T).FullName ?? typeof(T).Name;
        }
        public void Info(string message, params object[] args)
        {
            WriteLog("INFO", ConsoleColor.Green, message, args);
        }
        public void Warn(string message, params object[] args)
        {
            WriteLog("WARN", ConsoleColor.Yellow, message, args);
        }
        public void Error(string message, Exception? ex, params object[] args)
        {
            WriteLog("ERROR", ConsoleColor.Red, message, args);
            if (ex != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ResetColor();
            }
        }
        public void Debug(string message, params object[] args)
        {
            WriteLog("DEBUG", ConsoleColor.Cyan, message, args);
        }
        private void WriteLog(string level, ConsoleColor color, string message, object[] args)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            var formattedMessage = args.Length > 0 ? string.Format(message, args) : message;
            Console.ForegroundColor = color;
            Console.WriteLine($"[{timestamp}] [{level}] [{_categoryName}] {formattedMessage}");
            Console.ResetColor();
        }
    }
}