using Beyond.Todo.Application.Abstractions;
using Beyond.Todo.Application.UseCases;
using Beyond.Todo.Domain.Entities;
using Moq;
using NUnit.Framework;

namespace Beyond.Todo.Application.Tests;

[TestFixture]
public class UseCaseTests
{
    [Test]
    public async Task CreateTodoItemHandler_CreatesItem()
    {
        var todoItemRepository = new Mock<ITodoItemRepository>();
        todoItemRepository.Setup(r => r.LoadAsync()).ReturnsAsync(new List<TodoItem>());
        todoItemRepository.Setup(r => r.SaveAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

        var todoListRepository = new Mock<ITodoListRepository>();
        todoListRepository.Setup(r => r.GetAllCategories()).ReturnsAsync(new List<string> { "Work" });
        todoListRepository.Setup(r => r.GetNextId()).ReturnsAsync(1);

        var distributedLock = new Mock<IAsyncDisposable>();
        distributedLock.Setup(l => l.DisposeAsync()).Returns(ValueTask.CompletedTask);

        var lockService = new Mock<IDataBaseDistributedLockService>();
        lockService.Setup(s => s.AcquireLockAsync(It.IsAny<string>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(distributedLock.Object);

        var handler = new CreateTodoItemHandler(todoItemRepository.Object, todoListRepository.Object, lockService.Object);

        var result = await handler.Handle(new CreateTodoItemCommand("Title", "Description", "Work"));

        Assert.That(result.Id, Is.EqualTo(1));
        Assert.That(result.Title, Is.EqualTo("Title"));
        todoItemRepository.Verify(r => r.SaveAsync(It.Is<TodoItem>(t => t.Id == 1 && t.Title == "Title")), Times.Once);
        distributedLock.Verify(l => l.DisposeAsync(), Times.Once);
    }

    [Test]
    public void CreateTodoItemHandler_ThrowsWhenCategoryMissing()
    {
        var todoItemRepository = new Mock<ITodoItemRepository>();
        todoItemRepository.Setup(r => r.LoadAsync()).ReturnsAsync(new List<TodoItem>());

        var todoListRepository = new Mock<ITodoListRepository>();
        todoListRepository.Setup(r => r.GetAllCategories()).ReturnsAsync(new List<string>());
        var lockService = new Mock<IDataBaseDistributedLockService>();

        var handler = new CreateTodoItemHandler(todoItemRepository.Object, todoListRepository.Object, lockService.Object);

        Assert.That(async () => await handler.Handle(new CreateTodoItemCommand("Title", "Description", "Work")),
            Throws.ArgumentException.With.Message.Contains("does not exist."));
    }

    [Test]
    public async Task UpdateTodoItemDescriptionHandler_UpdatesDescription()
    {
        var items = new List<TodoItem> { new(1, "Title", "Description", "Category") };
        var repository = new Mock<ITodoItemRepository>();
        repository.Setup(r => r.LoadAsync()).ReturnsAsync(items);
        repository.Setup(r => r.SaveAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

        var handler = new UpdateTodoItemDescriptionHandler(repository.Object);

        var result = await handler.Handle(new UpdateTodoItemDescriptionCommand(1, "Updated"));

        Assert.That(result.UpdatedDescription, Is.EqualTo("Updated"));
        repository.Verify(r => r.SaveAsync(It.Is<TodoItem>(t => t.Description == "Updated")), Times.Once);
    }

    [Test]
    public async Task RegisterTodoItemProgressionHandler_AddsProgression()
    {
        var items = new List<TodoItem> { new(1, "Title", "Description", "Category") };
        var repository = new Mock<ITodoItemRepository>();
        repository.Setup(r => r.LoadAsync()).ReturnsAsync(items);
        repository.Setup(r => r.SaveAsync(It.IsAny<TodoItem>())).Returns(Task.CompletedTask);

        var handler = new RegisterTodoItemProgressionHandler(repository.Object);

        var result = await handler.Handle(new RegisterTodoItemProgressionCommand(1, DateTime.UtcNow, 20));

        Assert.That(result.Percent, Is.EqualTo(20));
        repository.Verify(r => r.SaveAsync(It.Is<TodoItem>(t => t.Progressions.Count == 1)), Times.Once);
    }

    [Test]
    public async Task RemoveTodoItemHandler_RemovesItem()
    {
        var items = new List<TodoItem> { new(1, "Title", "Description", "Category") };
        var repository = new Mock<ITodoItemRepository>();
        repository.Setup(r => r.LoadAsync()).ReturnsAsync(items);
        repository.Setup(r => r.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);

        var handler = new RemoveTodoItemHandler(repository.Object);

        var result = await handler.Handle(new RemoveTodoItemCommand(1));

        Assert.That(result.Id, Is.EqualTo(1));
        repository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Test]
    public async Task GetTodoItemsHandler_ReturnsMappedItems()
    {
        var item = new TodoItem(1, "Title", "Description", "Category");
        item.AddProgression(new Progression(DateTime.UtcNow, 10));
        var repository = new Mock<ITodoItemRepository>();
        repository.Setup(r => r.LoadAsync()).ReturnsAsync(new List<TodoItem> { item });

        var handler = new GetTodoItemsHandler(repository.Object);

        var result = await handler.Handle();

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result.First().Progressions.First().Percent, Is.EqualTo(10));
    }

    [Test]
    public async Task GetTodoItemsCategoriesHandler_ReturnsFromCacheWhenAvailable()
    {
        var cacheService = new Mock<ICategoryCacheService>();
        cacheService.Setup(c => c.GetCategoriesAsync()).ReturnsAsync(new List<string> { "Cached" });

        var repository = new Mock<ITodoListRepository>();

        var handler = new GetTodoItemsCategoriesHandler(repository.Object, cacheService.Object);

        var categories = await handler.Handle();

        Assert.That(categories.Single(), Is.EqualTo("Cached"));
        repository.Verify(r => r.GetAllCategories(), Times.Never);
    }

    [Test]
    public async Task GetTodoItemsCategoriesHandler_LoadsFromRepositoryWhenCacheEmpty()
    {
        var cacheService = new Mock<ICategoryCacheService>();
        cacheService.Setup(c => c.GetCategoriesAsync()).ReturnsAsync(new List<string>());
        cacheService.Setup(c => c.SetCategoriesAsync(It.IsAny<List<string>>())).Returns(Task.CompletedTask);

        var repository = new Mock<ITodoListRepository>();
        repository.Setup(r => r.GetAllCategories()).ReturnsAsync(new List<string> { "Repo" });

        var handler = new GetTodoItemsCategoriesHandler(repository.Object, cacheService.Object);

        var categories = await handler.Handle();

        Assert.That(categories.Single(), Is.EqualTo("Repo"));
        cacheService.Verify(c => c.SetCategoriesAsync(It.Is<List<string>>(l => l.Single() == "Repo")), Times.Once);
    }
}
