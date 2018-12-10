// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class HelloCommand : ICommand
    {
        public void AcceptVisitor(CommandVisitorBase visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(CommandVisitorBase<T> visitor) => visitor.Visit(this);
    }

    public partial class CommandVisitorBase
    {
        protected internal virtual void Visit(HelloCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T>
    {
        protected internal virtual T Visit(HelloCommand command) => Default(command);
    }
}
