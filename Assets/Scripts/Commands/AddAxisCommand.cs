namespace XFlag.Alter3Simulator
{
    public partial class AddAxisCommand
    {
        public AxisParam Param { get; set; }

        public override string ToString()
        {
            return $"{GetType()}{{Param={Param}}}";
        }
    }
}
