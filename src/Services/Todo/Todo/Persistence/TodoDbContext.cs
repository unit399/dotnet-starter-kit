using Finbuckle.MultiTenant.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ROC.Core.Infrastructure.Persistence;
using ROC.Core.Infrastructure.Tenant;
using ROC.WebApi.Core.Persistence;
using ROC.WebApi.Todo.Entities;

namespace ROC.WebApi.Todo.Persistence;

public sealed class TodoDbContext : RocDbContext
{
    public TodoDbContext(IMultiTenantContextAccessor<RocTenantInfo> multiTenantContextAccessor,
        DbContextOptions<TodoDbContext> options, IPublisher publisher, IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, publisher, settings)
    {
    }

    public DbSet<TodoItem> Todos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
        modelBuilder.HasDefaultSchema(SchemaNames.Todo);
    }
}