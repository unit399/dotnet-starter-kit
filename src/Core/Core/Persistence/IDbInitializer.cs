using System.Threading;
using System.Threading.Tasks;

namespace ROC.WebApi.Core.Persistence;

public interface IDbInitializer
{
    Task MigrateAsync(CancellationToken cancellationToken);
    Task SeedAsync(CancellationToken cancellationToken);
}