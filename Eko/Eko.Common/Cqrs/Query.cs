namespace Eko.Common.Cqrs;

public interface IQuery<TResult>
{

}

public interface IQueryHandler<TCommand, TResult> where TCommand : IQuery<TResult>
{
    Task<TResult> Execute(TCommand command);
}