using System.Collections.ObjectModel;
using System.Net;

namespace ROC.WebApi.Core.Exceptions;

public class ForbiddenException : BaseException
{
    public ForbiddenException()
        : base("unauthorized", new Collection<string>(), HttpStatusCode.Forbidden)
    {
    }
    public ForbiddenException(string message)
        : base(message, new Collection<string>(), HttpStatusCode.Forbidden)
    {
    }
}