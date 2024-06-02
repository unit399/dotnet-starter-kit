using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ROC.WebApi.Todo.Persistence;

namespace ROC.WebApi.Todo.Handlers.Create.v1;

public class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoValidator(TodoDbContext context)
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .MustAsync(async (title, token) =>
                await context.Todos.AllAsync(a => a.Title != title, token).ConfigureAwait(false))
            .WithMessage((_, name) => "todo item title '{PropertyValue}' already exists.");

        RuleFor(p => p.Note).NotEmpty();
    }
}