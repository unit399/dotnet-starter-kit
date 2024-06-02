using MediatR;
using Microsoft.Extensions.Logging;
using ROC.WebApi.Core.Cache;
using ROC.WebApi.Core.Entities.Events;
using ROC.WebApi.Todo.Handlers.Get.v1;

namespace ROC.WebApi.Todo.Events;

public record TodoItemCreated(Guid Id, string Title, string Notes) : DomainEvent;

public class TodoItemCreatedEventHandler(
    ILogger<TodoItemCreatedEventHandler> logger,
    ICacheService cache)
    : INotificationHandler<TodoItemCreated>
{
    public async Task Handle(TodoItemCreated notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("handling todo item created domain event..");
        var cacheResponse = new GetTodoResponse(notification.Id, notification.Title, notification.Notes);
        await cache.SetAsync($"todo:{notification.Id}", cacheResponse, cancellationToken: cancellationToken);
    }
}