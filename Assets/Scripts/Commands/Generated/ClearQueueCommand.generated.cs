// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class ClearQueueCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(ClearQueueCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(ClearQueueCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(ClearQueueCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(ClearQueueCommand command) => Default(command);
    }
}
