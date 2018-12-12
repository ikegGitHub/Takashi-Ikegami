// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class QuitCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(QuitCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(QuitCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(QuitCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(QuitCommand command) => Default(command);
    }
}
