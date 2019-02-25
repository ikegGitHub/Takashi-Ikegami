namespace XFlag.Alter3Simulator
{
    public class CommandTimelineRecord
    {
        private byte _axisNumber;
        private byte _value;

        public long MicroSeconds { get; }

        public CommandTimelineRecord(long microSeconds, byte axisNumber, byte value)
        {
            MicroSeconds = microSeconds;
            _axisNumber = axisNumber;
            _value = value;
        }

        public MoveAxisCommand ToMoveAxisCommand()
        {
            return new MoveAxisCommand { Param = new AxisParam { AxisNumber = _axisNumber, Value = _value, Priority = 9 } };
        }

        public override string ToString()
        {
            return $"{MicroSeconds} [us], moveaxis {_axisNumber} {_value} 9";
        }
    }
}
