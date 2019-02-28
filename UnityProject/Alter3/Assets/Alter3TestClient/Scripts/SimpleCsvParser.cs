using System;
using System.Collections.Generic;
using System.IO;

namespace XFlag.Alter3Simulator
{
    public class SimpleCsvParser : IDisposable
    {
        private TextReader _reader;

        public SimpleCsvParser(TextReader reader)
        {
            _reader = reader;
        }

        public string[][] Parse()
        {
            var csv = new List<string[]>();

            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                csv.Add(line.Trim().Split(','));
            }

            return csv.ToArray();
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}
