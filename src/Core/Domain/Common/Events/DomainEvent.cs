using System;

namespace ROC.WebApi.Domain.Common.Events;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime TriggeredAt { get; protected set; } = DateTime.UtcNow;
}