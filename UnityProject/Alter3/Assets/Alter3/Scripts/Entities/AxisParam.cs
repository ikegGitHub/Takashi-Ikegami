namespace XFlag.Alter3Simulator
{
    public class AxisParam
    {
        public int AxisNumber { get; set; }

        public double Value { get; set; }

        public int Priority { get; set; }

        public int Duration { get; set; }

        public override string ToString()
        {
            return $"{GetType()}{{AxisNumber={AxisNumber},Value={Value},Priority={Priority},Duration={Duration}}}";
        }
    }
}
