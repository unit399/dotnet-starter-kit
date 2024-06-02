using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ROC.WebApi.Core.Paging;
using ROC.WebApi.Core.Persistence;
using ROC.WebApi.Core.Specifications;
using ROC.WebApi.Todo.Entities;

namespace ROC.WebApi.Todo.Handlers.GetList.v1;

public sealed class GetTodoListHandler(
    [FromKeyedServices("todo")] IReadRepository<TodoItem> repository)
    : IRequestHandler<GetTodoListRequest, PagedList<TodoDto>>
{
    public async Task<PagedList<TodoDto>> Handle(GetTodoListRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var spec = new ListSpecification<TodoItem, TodoDto>(request.PageNumber, request.PageSize);
        var items = await repository.PaginatedListAsync(spec, request.PageNumber, request.PageSize, cancellationToken)
            .ConfigureAwait(false);
        return items;
    }
}