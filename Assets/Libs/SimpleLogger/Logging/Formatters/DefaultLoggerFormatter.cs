namespace SimpleLogger.Logging.Formatters
{
    public class DefaultLoggerFormatter : ILoggerFormatter
    {
        public string ApplyFormat(LogMessage logMessage)
        {
            return string.Format("{0:dd.MM.yyyy HH:mm}: {1} [line: {2} {3} -> {4}()]: {5}",
                            logMessage.DateTime, logMessage.Level, logMessage.LineNumber, logMessage.CallingClass,
                            logMessage.CallingMethod, logMessage.Text);
        }
    }
}