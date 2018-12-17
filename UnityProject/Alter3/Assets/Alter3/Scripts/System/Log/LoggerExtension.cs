using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public static class LoggerExtension
    {
        public static ILogHandler And(this ILogHandler logger, ILogHandler anotherLogger)
        {
            return new CombineLogger(logger, anotherLogger);
        }
    }
}
