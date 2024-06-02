using MediatR;

namespace ROC.WebApi.Todo.Handlers.Get.v1;

public class GetTodoRequest : IRequest<GetTodoResponse>
{
    public GetTodoRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}