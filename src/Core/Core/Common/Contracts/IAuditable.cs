using System;

namespace ROC.WebApi.Core.Common.Contracts;

public interface IAuditable
{
    Guid CreatedBy { get; set; }
    DateTimeOffset CreatedAt { get; }
    Guid? UpdatedBy { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}