using System.Collections.ObjectModel;
using ROC.WebApi.Core.Entities.Events;

namespace ROC.WebApi.Core.Entities.Contracts;

public interface IEntity
{
    Collection<DomainEvent> DomainEvents { get; }
}

public interface IEntity<out TId> : IEntity
{
    TId Id { get; }
}