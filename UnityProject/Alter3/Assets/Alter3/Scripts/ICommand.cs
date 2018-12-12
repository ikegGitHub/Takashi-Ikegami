namespace XFlag.Alter3Simulator
{
    public interface ICommand
    {
        void AcceptVisitor(ICommandVisitor visitor);
        T AcceptVisitor<T>(ICommandVisitor<T> visitor);
    }
}
