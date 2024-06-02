using ROC.WebApi.Core.Entities.Events;

namespace ROC.WebApi.Catalog.Domain.Events;

public sealed record ProductCreated : DomainEvent
{
    public Product? Product { get; set; }
}