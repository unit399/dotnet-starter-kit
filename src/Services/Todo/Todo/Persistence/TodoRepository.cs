using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Mapster;
using ROC.WebApi.Core.Entities.Contracts;
using ROC.WebApi.Core.Persistence;

namespace ROC.WebApi.Todo.Persistence;

internal sealed class TodoRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public TodoRepository(TodoDbContext context)
        : base(context)
    {
    }

    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
    }
}