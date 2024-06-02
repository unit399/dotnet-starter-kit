using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using ROC.Core.Infrastructure.Auth.Policy;

namespace ROC.WebApi.Todo.Handlers.Get.v1;

public static class GetTodoEndpoint
{
    internal static RouteHandlerBuilder MapGetTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", (Guid id, ISender mediator) => mediator.Send(new GetTodoRequest(id)))
            .WithName(nameof(GetTodoEndpoint))
            .WithSummary("gets todo item by id")
            .WithDescription("gets todo item by id")
            .Produces<GetTodoResponse>()
            .RequirePermission("Permissions.Todos.View")
            .MapToApiVersion(1);
    }
}