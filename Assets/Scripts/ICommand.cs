namespace XFlag.Alter3Simulator
{
    public interface ICommand
    {
        void AcceptVisitor(CommandVisitorBase visitor);
        T AcceptVisitor<T>(CommandVisitorBase<T> visitor);
    }
}
