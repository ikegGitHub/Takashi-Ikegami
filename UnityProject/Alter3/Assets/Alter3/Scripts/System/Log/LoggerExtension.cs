namespace XFlag.Alter3Simulator
{
    public static class LoggerExtension
    {
        public static ILogger And(this ILogger logger, ILogger anotherLogger)
        {
            return new CombineLogger(logger, anotherLogger);
        }
    }
}
