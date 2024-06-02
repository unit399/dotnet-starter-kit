using Carter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using ROC.Core.Infrastructure.Persistence;
using ROC.WebApi.Core.Persistence;
using ROC.WebApi.Todo.Entities;
using ROC.WebApi.Todo.Handlers.Create.v1;
using ROC.WebApi.Todo.Handlers.Get.v1;
using ROC.WebApi.Todo.Handlers.GetList.v1;
using ROC.WebApi.Todo.Persistence;

namespace ROC.WebApi.Todo;

public static class TodoModule
{
    public static WebApplicationBuilder RegisterTodoServices(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.Services.BindDbContext<TodoDbContext>();
        builder.Services.AddScoped<IDbInitializer, TodoDbInitializer>();
        builder.Services.AddKeyedScoped<IRepository<TodoItem>, TodoRepository<TodoItem>>("todo");
        builder.Services.AddKeyedScoped<IReadRepository<TodoItem>, TodoRepository<TodoItem>>("todo");
        return builder;
    }

    public static WebApplication UseTodoModule(this WebApplication app)
    {
        return app;
    }

    public class Endpoints : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            var todoGroup = app.MapGroup("todos").WithTags("todos");
            todoGroup.MapTodoItemCreationEndpoint();
            todoGroup.MapGetTodoEndpoint();
            todoGroup.MapGetTodoListEndpoint();
        }
    }
}