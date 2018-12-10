namespace XFlag.Alter3Simulator
{
    public partial class CommandVisitorBase
    {
        protected internal virtual void Default(ICommand command) { }
    }

    public partial class CommandVisitorBase<T>
    {
        protected internal virtual T Default(ICommand command) => default(T);
    }
}
