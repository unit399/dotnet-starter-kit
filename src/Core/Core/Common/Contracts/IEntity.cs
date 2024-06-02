using System.Collections.ObjectModel;
using ROC.WebApi.Core.Common.Events;

namespace ROC.WebApi.Core.Common.Contracts;

public interface IEntity
{
    Collection<DomainEvent> DomainEvents { get; }
}

public interface IEntity<out TId> : IEntity
{
    TId Id { get; }
}