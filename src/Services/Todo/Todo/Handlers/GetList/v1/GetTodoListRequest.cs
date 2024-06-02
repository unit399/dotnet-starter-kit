using MediatR;
using ROC.WebApi.Core.Paging;

namespace ROC.WebApi.Todo.Handlers.GetList.v1;

public record GetTodoListRequest(int PageNumber, int PageSize) : IRequest<PagedList<TodoDto>>;