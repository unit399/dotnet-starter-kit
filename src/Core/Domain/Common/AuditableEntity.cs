using System;
using ROC.WebApi.Domain.Common.Contracts;

namespace ROC.WebApi.Domain.Common;

public class AuditableEntity<TId> : BaseEntity<TId>, IAuditable, ISoftDeletable
{
    public Guid CreatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}

public abstract class AuditableEntity : AuditableEntity<Guid>
{
    protected AuditableEntity() => Id = Guid.NewGuid();
}
