// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class RobotInfoCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(RobotInfoCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(RobotInfoCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(RobotInfoCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(RobotInfoCommand command) => Default(command);
    }
}
