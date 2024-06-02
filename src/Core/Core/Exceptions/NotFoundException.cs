using System.Collections.ObjectModel;
using System.Net;

namespace ROC.WebApi.Core.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message)
        : base(message, new Collection<string>(), HttpStatusCode.NotFound)
    {
    }
}
