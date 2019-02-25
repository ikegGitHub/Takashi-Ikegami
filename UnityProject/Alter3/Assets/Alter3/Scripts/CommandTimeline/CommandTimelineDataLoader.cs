using System;
using System.IO;

namespace XFlag.Alter3Simulator
{
    public class CommandTimelineDataLoader : IDisposable
    {
        private BinaryReader _reader;

        public CommandTimelineDataLoader(BinaryReader reader)
        {
            _reader = reader;
        }

        public CommandTimelineRecord[] Load()
        {
            var count = _reader.ReadInt32();

            var commands = new CommandTimelineRecord[count];
            for (int i = 0; i < count; i++)
            {
                var microSeconds = _reader.ReadInt64();
                var axisNumber = _reader.ReadByte();
                var value = _reader.ReadByte();
                commands[i] = new CommandTimelineRecord(microSeconds, axisNumber, value);
            }
            return commands;
        }

        void IDisposable.Dispose()
        {
            _reader.Dispose();
        }
    }
}
