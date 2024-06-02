using ROC.WebApi.Catalog.Domain.Events;
using ROC.WebApi.Core.Entities;
using ROC.WebApi.Core.Entities.Contracts;

namespace ROC.WebApi.Catalog.Domain;

public class Product : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }

    public static Product Create(string name, string? description, decimal price)
    {
        var product = new Product();

        product.Name = name;
        product.Description = description;
        product.Price = price;

        product.QueueDomainEvent(new ProductCreated { Product = product });

        return product;
    }
}