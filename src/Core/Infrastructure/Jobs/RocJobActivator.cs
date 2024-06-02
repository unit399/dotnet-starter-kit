using System;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using ROC.Core.Infrastructure.Constants;
using ROC.Core.Infrastructure.Tenant;
using ROC.WebApi.Core.Identity.Users.Interfaces;
using ROC.WebApi.Core.Tenant;

namespace ROC.Core.Infrastructure.Jobs;

public class RocJobActivator : JobActivator
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RocJobActivator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    public override JobActivatorScope BeginScope(PerformContext context)
    {
        return new Scope(context, _scopeFactory.CreateScope());
    }

    private sealed class Scope : JobActivatorScope, IServiceProvider
    {
        private readonly PerformContext _context;
        private readonly IServiceScope _scope;

        public Scope(PerformContext context, IServiceScope scope)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _scope = scope ?? throw new ArgumentNullException(nameof(scope));

            ReceiveParameters();
        }

        object? IServiceProvider.GetService(Type serviceType)
        {
            return serviceType == typeof(PerformContext)
                ? _context
                : _scope.ServiceProvider.GetService(serviceType);
        }

        private void ReceiveParameters()
        {
            var tenantInfo = _context.GetJobParameter<RocTenantInfo>(TenantConstants.Identifier);
            if (tenantInfo is not null)
                _scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>()
                    .MultiTenantContext = new MultiTenantContext<RocTenantInfo>
                {
                    TenantInfo = tenantInfo
                };

            var userId = _context.GetJobParameter<string>(QueryStringKeys.UserId);
            if (!string.IsNullOrEmpty(userId))
                _scope.ServiceProvider.GetRequiredService<ICurrentUserInitializer>()
                    .SetCurrentUserId(userId);
        }

        public override object Resolve(Type type)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(this, type);
        }
    }
}