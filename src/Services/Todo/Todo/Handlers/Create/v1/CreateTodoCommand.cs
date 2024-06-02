using System.ComponentModel;
using MediatR;

namespace ROC.WebApi.Todo.Handlers.Create.v1;

public record CreateTodoCommand(
    [property: DefaultValue("Hello World!")]
    string Title,
    [property: DefaultValue("Important Note.")]
    string Note) : IRequest<CreateTodoRepsonse>;