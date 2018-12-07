// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class ResetPoseCommand : ICommand
    {
        public void AcceptVisitor(CommandVisitorBase visitor) => visitor.Visit(this);
        public T AcceptVisitor<T>(CommandVisitorBase<T> visitor) => visitor.Visit(this);
    }

    public partial class CommandVisitorBase
    {
        public virtual void Visit(ResetPoseCommand command) => Visit((ICommand)command);
    }

    public partial class CommandVisitorBase<T>
    {
        public virtual T Visit(ResetPoseCommand command) => Visit((ICommand)command);
    }
}
