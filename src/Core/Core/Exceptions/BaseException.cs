using System;
using System.Collections.Generic;
using System.Net;

namespace ROC.WebApi.Core.Exceptions;

public class BaseException : Exception
{
    public BaseException(string message, IEnumerable<string> errors,
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        : base(message)
    {
        ErrorMessages = errors;
        StatusCode = statusCode;
    }

    public BaseException(string message) : base(message)
    {
        ErrorMessages = new List<string>();
    }

    public IEnumerable<string> ErrorMessages { get; }

    public HttpStatusCode StatusCode { get; }
}