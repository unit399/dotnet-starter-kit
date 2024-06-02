﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ROC.WebApi.Core.Persistence;
using ROC.WebApi.Todo.Entities;

namespace ROC.WebApi.Todo.Handlers.Create.v1;

public sealed class CreateTodoHandler(
    ILogger<CreateTodoHandler> logger,
    [FromKeyedServices("todo")] IRepository<TodoItem> repository)
    : IRequestHandler<CreateTodoCommand, CreateTodoRepsonse>
{
    public async Task<CreateTodoRepsonse> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = TodoItem.Create(request.Title, request.Note);
        await repository.AddAsync(item, cancellationToken).ConfigureAwait(false);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("todo item created {TodoItemId}", item.Id);
        return new CreateTodoRepsonse(item.Id);
    }
}