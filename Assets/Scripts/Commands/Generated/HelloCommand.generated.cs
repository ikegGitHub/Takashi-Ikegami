// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class HelloCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(HelloCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(HelloCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(HelloCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(HelloCommand command) => Default(command);
    }
}
