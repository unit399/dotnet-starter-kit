using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using ROC.WebApi.Core.Entities.Contracts;
using ROC.WebApi.Core.Entities.Events;

namespace ROC.WebApi.Core.Entities;

public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; protected init; } = default!;

    [NotMapped] public Collection<DomainEvent> DomainEvents { get; } = new();

    public void QueueDomainEvent(DomainEvent @event)
    {
        if (!DomainEvents.Contains(@event))
            DomainEvents.Add(@event);
    }
}

public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}