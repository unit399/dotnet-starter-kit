using System;
using MediatR;

namespace ROC.WebApi.Core.Entities.Events;

public abstract record DomainEvent : IDomainEvent, INotification
{
    public DateTime TriggeredAt { get; protected set; } = DateTime.UtcNow;
}