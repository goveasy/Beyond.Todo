using Beyond.Todo.Domain.Aggregates;
using Beyond.Todo.Domain.Entities;
using NUnit.Framework;

namespace Beyond.Todo.Domain.Tests;

[TestFixture]
public class TodoListTests
{
    [Test]
    public void AddItem_AddsNewItem()
    {
        var todoList = new TodoList();

        todoList.AddItem(1, "Title", "Description", "Category");

        Assert.That(todoList.Items.Count, Is.EqualTo(1));
    }

    [Test]
    public void AddItem_ThrowsWhenDuplicateId()
    {
        var todoList = new TodoList(new List<TodoItem> { new(1, "Title", "Description", "Category") });

        Assert.That(() => todoList.AddItem(1, "Other", "Description", "Category"),
            Throws.InvalidOperationException.With.Message.Contain("TodoItem with id 1 already exists."));
    }

    [Test]
    public void UpdateItem_UpdatesExistingItem()
    {
        var todoList = new TodoList(new List<TodoItem> { new(1, "Title", "Description", "Category") });

        todoList.UpdateItem(1, "Updated");

        Assert.That(todoList.Items.First().Description, Is.EqualTo("Updated"));
    }

    [Test]
    public void RemoveItem_RemovesExistingItem()
    {
        var todoList = new TodoList(new List<TodoItem> { new(1, "Title", "Description", "Category") });

        todoList.RemoveItem(1);

        Assert.That(todoList.Items, Is.Empty);
    }

    [Test]
    public void RegisterProgression_AddsProgressionToItem()
    {
        var todoList = new TodoList(new List<TodoItem> { new(1, "Title", "Description", "Category") });

        todoList.RegisterProgression(1, DateTime.UtcNow, 30);

        Assert.That(todoList.Items.First().Progressions.Count, Is.EqualTo(1));
    }
}
