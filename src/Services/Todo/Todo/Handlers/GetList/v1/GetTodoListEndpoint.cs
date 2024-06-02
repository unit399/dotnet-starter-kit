using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ROC.Core.Infrastructure.Auth.Policy;
using ROC.WebApi.Core.Paging;

namespace ROC.WebApi.Todo.Handlers.GetList.v1;

public static class GetTodoListEndpoint
{
    internal static RouteHandlerBuilder MapGetTodoListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", (ISender mediator, int pageNumber = 1, int pageSize = 10) =>
                mediator.Send(new GetTodoListRequest(pageNumber, pageSize)))
            .WithName(nameof(GetTodoListEndpoint))
            .WithSummary("gets a list of todo items with paging support")
            .WithDescription("gets a list of todo items with paging support")
            .Produces<PagedList<TodoDto>>()
            .RequirePermission("Permissions.Todos.View")
            .MapToApiVersion(1);
    }
}