using System;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class UnityLogger : ILogger
    {
        public static readonly UnityLogger Instance = new UnityLogger();

        private UnityLogger()
        {
        }

        public void Info(string message)
        {
            Debug.Log($"l:INFO\tt:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\tm:{message}");
        }

        public void Error(string message)
        {
            Debug.Log($"l:ERROR\tt:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\tm:{message}");
        }
    }
}
