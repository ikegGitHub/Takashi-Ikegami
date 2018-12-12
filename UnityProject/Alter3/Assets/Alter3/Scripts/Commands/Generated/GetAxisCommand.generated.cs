// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class GetAxisCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(GetAxisCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(GetAxisCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(GetAxisCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(GetAxisCommand command) => Default(command);
    }
}
