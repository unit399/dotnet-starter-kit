using Ardalis.Specification;
using ROC.WebApi.Core.Entities.Contracts;

namespace ROC.WebApi.Core.Persistence;

public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
}

public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}