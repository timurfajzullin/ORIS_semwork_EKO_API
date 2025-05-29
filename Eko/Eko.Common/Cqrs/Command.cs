namespace Eko.Common.Cqrs;

public interface ICommand<TResult>
{

}

public interface ICommand
{

}


public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    Task<TResult> Execute(TCommand command);
}

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task Execute(TCommand command);
}