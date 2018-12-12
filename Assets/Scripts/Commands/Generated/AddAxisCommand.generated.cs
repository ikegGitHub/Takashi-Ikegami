// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class AddAxisCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(AddAxisCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(AddAxisCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(AddAxisCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(AddAxisCommand command) => Default(command);
    }
}
