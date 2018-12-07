namespace XFlag.Alter3Simulator
{
    public partial class CommandVisitorBase
    {
        public virtual void Visit(ICommand command)
        {
        }
    }

    public partial class CommandVisitorBase<T>
    {
        public virtual T Visit(ICommand command)
        {
            return default(T);
        }
    }
}
