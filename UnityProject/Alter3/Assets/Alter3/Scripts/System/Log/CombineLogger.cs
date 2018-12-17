using System;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class CombineLogger : ILogHandler
    {
        private readonly ILogHandler _logger1;
        private readonly ILogHandler _logger2;

        public CombineLogger(ILogHandler logger1, ILogHandler logger2)
        {
            _logger1 = logger1;
            _logger2 = logger2;
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            _logger1.LogFormat(logType, context, format, args);
            _logger2.LogFormat(logType, context, format, args);
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            _logger1.LogException(exception, context);
            _logger2.LogException(exception, context);
        }
    }
}
