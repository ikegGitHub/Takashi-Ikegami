using System.Collections.Generic;

namespace XFlag.Alter3Simulator
{
    public static class Response
    {
        public const string OK = "OK";

        public static IEnumerable<string> MakeErrorResponse(string message)
        {
            yield return $"ERROR: {message}";
        }
    }
}
