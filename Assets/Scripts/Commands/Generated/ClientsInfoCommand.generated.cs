// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class ClientsInfoCommand : ICommand
    {
        public void AcceptVisitor(CommandVisitorBase visitor) => visitor.Visit(this);
        public T AcceptVisitor<T>(CommandVisitorBase<T> visitor) => visitor.Visit(this);
    }

    public partial class CommandVisitorBase
    {
        public virtual void Visit(ClientsInfoCommand command) => Visit((ICommand)command);
    }

    public partial class CommandVisitorBase<T>
    {
        public virtual T Visit(ClientsInfoCommand command) => Visit((ICommand)command);
    }
}
