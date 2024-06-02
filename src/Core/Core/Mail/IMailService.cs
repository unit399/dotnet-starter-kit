using System.Threading;
using System.Threading.Tasks;

namespace ROC.WebApi.Core.Mail;

public interface IMailService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}