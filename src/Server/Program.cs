using ROC.Core.Infrastructure;
using ROC.Core.Infrastructure.Logging.Serilog;
using Serilog;
using Server;

StaticLogger.EnsureInitialized();
Log.Information("server booting up..");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.RegisterRocFramework();
    builder.RegisterModules();

    var app = builder.Build();
    app.UseRocFramework();
    app.UseModules();
    await app.RunAsync();
}
catch (Exception ex) when (!ex.GetType().Name.Equals("HostAbortedException", StringComparison.Ordinal))
{
    StaticLogger.EnsureInitialized();
    Log.Fatal(ex.Message, "unhandled exception");
}
finally
{
    StaticLogger.EnsureInitialized();
    Log.Information("server shutting down..");
    await Log.CloseAndFlushAsync();
}