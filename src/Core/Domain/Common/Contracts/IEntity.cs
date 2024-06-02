using System.Collections.Generic;
using System.Collections.ObjectModel;
using ROC.WebApi.Domain.Common.Events;

namespace ROC.WebApi.Domain.Common.Contracts;

public interface IEntity
{
    Collection<DomainEvent> DomainEvents { get; }
}

public interface IEntity<out TId> : IEntity
{
    TId Id { get; }
}