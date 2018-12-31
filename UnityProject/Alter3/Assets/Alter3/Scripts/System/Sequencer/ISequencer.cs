namespace XFlag.Alter3Simulator
{
    public interface ISequencer
    {
        uint Current { get; }

        uint Next();
    }
}
