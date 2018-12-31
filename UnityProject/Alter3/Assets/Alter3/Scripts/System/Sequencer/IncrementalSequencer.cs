namespace XFlag.Alter3Simulator
{
    public class IncrementalSequencer : ISequencer
    {
        private uint _id = 1;

        uint ISequencer.Current => _id;

        uint ISequencer.Next()
        {
            return _id++;
        }
    }
}
