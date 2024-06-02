﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ROC.WebApi.Core.Exceptions;
using Serilog.Context;

namespace ROC.Core.Infrastructure.Exceptions;

public class RocExceptionHandler(ILogger<RocExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        ArgumentNullException.ThrowIfNull(exception);
        var problemDetails = new ProblemDetails();
        problemDetails.Instance = httpContext.Request.Path;

        if (exception is ValidationException fluentException)
        {
            problemDetails.Detail = "one or more validation errors occurred";
            problemDetails.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            var validationErrors = new List<string>();
            foreach (var error in fluentException.Errors) validationErrors.Add(error.ErrorMessage);
            problemDetails.Extensions.Add("errors", validationErrors);
        }

        else if (exception is BaseException e)
        {
            httpContext.Response.StatusCode = (int)e.StatusCode;
            problemDetails.Detail = e.Message;
            if (e.ErrorMessages != null && e.ErrorMessages.Any())
                problemDetails.Extensions.Add("errors", e.ErrorMessages);
        }

        else
        {
            problemDetails.Detail = exception.Message;
        }

        LogContext.PushProperty("StackTrace", exception.StackTrace);
        logger.LogError("{ProblemDetail}", problemDetails.Detail);
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        return true;
    }
}