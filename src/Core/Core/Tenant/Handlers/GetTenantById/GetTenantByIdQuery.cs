using MediatR;
using ROC.WebApi.Core.Tenant.Entities;

namespace ROC.WebApi.Core.Tenant.Handlers.GetTenantById;

public record GetTenantByIdQuery(string TenantId) : IRequest<TenantDetail>;