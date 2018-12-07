// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class RobotInfoCommand : ICommand
    {
        public void AcceptVisitor(CommandVisitorBase visitor) => visitor.Visit(this);
        public T AcceptVisitor<T>(CommandVisitorBase<T> visitor) => visitor.Visit(this);
    }

    public partial class CommandVisitorBase
    {
        public virtual void Visit(RobotInfoCommand command) => Visit((ICommand)command);
    }

    public partial class CommandVisitorBase<T>
    {
        public virtual T Visit(RobotInfoCommand command) => Visit((ICommand)command);
    }
}
