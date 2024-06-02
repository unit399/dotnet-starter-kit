using System;

namespace ROC.WebApi.Core.Common.Events;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime TriggeredAt { get; protected set; } = DateTime.UtcNow;
}