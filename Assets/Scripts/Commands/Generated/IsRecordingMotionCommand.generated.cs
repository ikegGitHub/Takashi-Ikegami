// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class IsRecordingMotionCommand : ICommand
    {
        public void AcceptVisitor(ICommandVisitor visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(ICommandVisitor<T> visitor) => visitor.Visit(this);
    }

    public partial interface ICommandVisitor
    {
        void Visit(IsRecordingMotionCommand command);
    }

    public partial interface ICommandVisitor<T>
    {
        T Visit(IsRecordingMotionCommand command);
    }

    public partial class CommandVisitorBase : ICommandVisitor
    {
        public virtual void Visit(IsRecordingMotionCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T> : ICommandVisitor<T>
    {
        public virtual T Visit(IsRecordingMotionCommand command) => Default(command);
    }
}
