// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class IsConnectedCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(IsConnectedCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(IsConnectedCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(IsConnectedCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(IsConnectedCommand command) => Default(command);
    }
}
