﻿// AUTO-GENERATED
namespace XFlag.Alter3Simulator
{
    public partial class AppendAxisCommand : ICommand
    {
        public void AcceptVisitor(CommandVisitorBase visitor) => visitor.Visit(this);

        public T AcceptVisitor<T>(CommandVisitorBase<T> visitor) => visitor.Visit(this);
    }

    public partial class CommandVisitorBase
    {
        protected internal virtual void Visit(AppendAxisCommand command) => Default(command);
    }

    public partial class CommandVisitorBase<T>
    {
        protected internal virtual T Visit(AppendAxisCommand command) => Default(command);
    }
}
