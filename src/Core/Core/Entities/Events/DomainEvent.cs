using System;

namespace ROC.WebApi.Core.Entities.Events;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime TriggeredAt { get; protected set; } = DateTime.UtcNow;
}