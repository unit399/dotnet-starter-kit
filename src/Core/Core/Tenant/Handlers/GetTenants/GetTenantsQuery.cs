using System.Collections.Generic;
using MediatR;
using ROC.WebApi.Core.Tenant.Entities;

namespace ROC.WebApi.Core.Tenant.Handlers.GetTenants;

public sealed class GetTenantsQuery : IRequest<List<TenantDetail>>;