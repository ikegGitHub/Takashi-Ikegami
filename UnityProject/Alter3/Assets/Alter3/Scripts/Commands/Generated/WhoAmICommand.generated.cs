// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class WhoAmICommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(WhoAmICommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(WhoAmICommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(WhoAmICommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(WhoAmICommand command) => Default(command);
    }
}
