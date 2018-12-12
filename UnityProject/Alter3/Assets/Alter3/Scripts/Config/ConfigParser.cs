using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace XFlag.Alter3Simulator
{
    public class ConfigParser
    {
        private static readonly Regex PropertyPattern = new Regex(@"\s*=\s*");

        private TextReader _reader;

        public ConfigParser(TextReader reader)
        {
            _reader = reader;
        }

        public IDictionary<string, string> Parse()
        {
            var properties = new Dictionary<string, string>();

            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length == 0 || line[0] == '#')
                {
                    continue;
                }
                var entry = PropertyPattern.Split(line, 2);
                properties[entry[0]] = entry[1];
            }

            return properties;
        }

        public static IDictionary<string, string> Parse(string text)
        {
            using (var reader = new StringReader(text))
            {
                return new ConfigParser(reader).Parse();
            }
        }

        public static IDictionary<string, string> Parse(TextAsset textAsset)
        {
            return Parse(textAsset.text);
        }
    }
}
