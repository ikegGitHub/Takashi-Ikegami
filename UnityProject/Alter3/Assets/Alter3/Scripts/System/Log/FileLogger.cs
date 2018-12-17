using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class FileLogger : ILogHandler, IDisposable
    {
        private TextWriter _writer;

        public FileLogger(string filePath, bool append = false)
        {
            _writer = new StreamWriter(filePath, append, new UTF8Encoding(false, false));
        }

        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            _writer.WriteLine($"l:{logType}\tt:{LogUtil.Timestamp()}\tm:{string.Format(format, args)}");
            _writer.Flush();
        }

        public void LogException(Exception exception, UnityEngine.Object context)
        {
            _writer.WriteLine($"l:{LogType.Exception}\tt:{LogUtil.Timestamp()}\tm:{exception.Message}");
            _writer.Flush();
        }

        public void Close()
        {
            _writer.Close();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
