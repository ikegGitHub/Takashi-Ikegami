﻿using System.Collections.Generic;

namespace XFlag.Alter3Simulator
{
    public static class Response
    {
        public const string OK = "OK";

        public static string[] MakeErrorResponse(string message)
        {
            return new string[] { $"ERROR: {message}" };
        }
    }
}
