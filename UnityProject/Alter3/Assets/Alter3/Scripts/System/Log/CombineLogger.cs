namespace XFlag.Alter3Simulator
{
    public class CombineLogger : ILogger
    {
        private readonly ILogger _logger1;
        private readonly ILogger _logger2;

        public CombineLogger(ILogger logger1, ILogger logger2)
        {
            _logger1 = logger1;
            _logger2 = logger2;
        }

        public void Info(string message)
        {
            _logger1.Info(message);
            _logger2.Info(message);
        }

        public void Error(string message)
        {
            _logger1.Error(message);
            _logger2.Error(message);
        }
    }
}
