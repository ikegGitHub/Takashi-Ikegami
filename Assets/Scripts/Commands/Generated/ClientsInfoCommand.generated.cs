// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class ClientsInfoCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(ClientsInfoCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(ClientsInfoCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(ClientsInfoCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(ClientsInfoCommand command) => Default(command);
    }
}
