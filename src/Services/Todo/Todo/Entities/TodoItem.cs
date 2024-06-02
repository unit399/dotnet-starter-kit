using ROC.WebApi.Core.Entities;
using ROC.WebApi.Core.Entities.Contracts;
using ROC.WebApi.Todo.Events;

namespace ROC.WebApi.Todo.Entities;

public class TodoItem : AuditableEntity, IAggregateRoot
{
    public string? Title { get; set; }

    public string? Note { get; set; }

    public static TodoItem Create(string title, string note)
    {
        var item = new TodoItem();

        item.Title = title;
        item.Note = note;

        item.QueueDomainEvent(new TodoItemCreated(item.Id, item.Title, item.Note));

        return item;
    }
}