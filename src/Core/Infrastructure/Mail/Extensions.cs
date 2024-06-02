using Microsoft.Extensions.DependencyInjection;
using ROC.WebApi.Core.Mail;

namespace ROC.Core.Infrastructure.Mail;

internal static class Extensions
{
    internal static IServiceCollection ConfigureMailing(this IServiceCollection services)
    {
        services.AddTransient<IMailService, SmtpMailService>();
        services.AddOptions<MailOptions>().BindConfiguration(nameof(MailOptions));
        return services;
    }
}