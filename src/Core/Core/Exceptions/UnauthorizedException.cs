using System.Collections.ObjectModel;
using System.Net;

namespace ROC.WebApi.Core.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException()
        : base("authentication failed", new Collection<string>(), HttpStatusCode.Unauthorized)
    {
    }
    public UnauthorizedException(string message)
        : base(message, new Collection<string>(), HttpStatusCode.Unauthorized)
    {
    }
}
