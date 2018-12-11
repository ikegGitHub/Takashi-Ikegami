using System;
using System.IO;
using System.Text;

namespace XFlag.Alter3Simulator
{
    public class FileLogger : ILogger, IDisposable
    {
        private TextWriter _writer;

        public FileLogger(string filePath, bool append = false)
        {
            _writer = new StreamWriter(filePath, append, new UTF8Encoding(false, false));
        }

        public void Info(string message)
        {
            _writer.WriteLine($"l:INFO\tt:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\tm:{message}");
            _writer.Flush();
        }

        public void Error(string message)
        {
            _writer.WriteLine($"l:ERROR\tt:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\tm:{message}");
            _writer.Flush();
        }

        public void Close()
        {
            _writer.Close();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 重複する呼び出しを検出するには

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
