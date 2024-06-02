namespace ROC.WebApi.Todo.Handlers.GetList.v1;

public record TodoDto(Guid? Id, string Title, string Note);