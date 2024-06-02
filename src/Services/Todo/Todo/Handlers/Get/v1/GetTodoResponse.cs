namespace ROC.WebApi.Todo.Handlers.Get.v1;

public record GetTodoResponse(Guid? Id, string Title, string Notes);