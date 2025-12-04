using Beyond.Todo.Domain.Entities;
using NUnit.Framework;

namespace Beyond.Todo.Domain.Tests;

[TestFixture]
public class TodoItemTests
{
    [Test]
    public void Constructor_SetsProperties()
    {
        var item = new TodoItem(1, "Title", "Description", "Category");

        Assert.That(item.Id, Is.EqualTo(1));
        Assert.That(item.Title, Is.EqualTo("Title"));
        Assert.That(item.Description, Is.EqualTo("Description"));
        Assert.That(item.Category, Is.EqualTo("Category"));
        Assert.That(item.IsCompleted, Is.False);
    }

    [Test]
    public void Constructor_ThrowsWhenTitleIsEmpty()
    {
        Assert.That(() => new TodoItem(1, string.Empty, "Description", "Category"),
            Throws.ArgumentException.With.Message.Contains("Title is required"));
    }

    [Test]
    public void UpdateDescription_UpdatesValue()
    {
        var item = new TodoItem(1, "Title", "Description", "Category");

        item.UpdateDescription("New Description");

        Assert.That(item.Description, Is.EqualTo("New Description"));
    }

    [Test]
    public void UpdateDescription_ThrowsWhenLocked()
    {
        var item = new TodoItem(1, "Title", "Description", "Category");
        item.AddProgression(new Progression(DateTime.UtcNow, 60));

        Assert.That(() => item.UpdateDescription("Updated"),
            Throws.InvalidOperationException.With.Message.EqualTo("Cannot update completed or half-completed items."));
    }

    [Test]
    public void RemoveGuard_ThrowsWhenLocked()
    {
        var item = new TodoItem(1, "Title", "Description", "Category");
        item.AddProgression(new Progression(DateTime.UtcNow, 80));

        Assert.That(() => item.RemoveGuard(),
            Throws.InvalidOperationException.With.Message.EqualTo("Cannot remove completed or half-completed items."));
    }

    [Test]
    public void AddProgression_AddsProgression()
    {
        var item = new TodoItem(1, "Title", "Description", "Category");
        var progression = new Progression(DateTime.UtcNow, 40);

        item.AddProgression(progression);

        Assert.That(item.Progressions.Count, Is.EqualTo(1));
        Assert.That(item.GetCumulativePercent(), Is.EqualTo(40));
    }

    [Test]
    public void AddProgression_ThrowsWhenDateNotIncreasing()
    {
        var date = DateTime.UtcNow;
        var item = new TodoItem(1, "Title", "Description", "Category");
        item.AddProgression(new Progression(date, 20));

        Assert.That(() => item.AddProgression(new Progression(date, 20)),
            Throws.InvalidOperationException.With.Message.EqualTo("Progression date must be greater than existing ones."));
    }

    [Test]
    public void AddProgression_ThrowsWhenExceedsHundred()
    {
        var item = new TodoItem(1, "Title", "Description", "Category");
        item.AddProgression(new Progression(DateTime.UtcNow, 80));

        Assert.That(() => item.AddProgression(new Progression(DateTime.UtcNow.AddMinutes(1), 30)),
            Throws.InvalidOperationException.With.Message.EqualTo("Total progression cannot exceed 100%."));
    }

    [Test]
    public void AddProgression_ThrowsWhenCompleted()
    {
        var item = new TodoItem(1, "Title", "Description", "Category");
        item.AddProgression(new Progression(DateTime.UtcNow, 80));
        item.AddProgression(new Progression(DateTime.UtcNow.AddMinutes(1), 20));

        Assert.That(() => item.AddProgression(new Progression(DateTime.UtcNow.AddMinutes(2), 10)),
            Throws.InvalidOperationException.With.Message.EqualTo("Item is already completed."));
    }
}
