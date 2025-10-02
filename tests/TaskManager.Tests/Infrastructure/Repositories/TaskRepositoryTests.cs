using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Tests.Infrastructure.Repositories;

public class TaskRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public async System.Threading.Tasks.Task AddAsync_ValidTask_ShouldAddTask()
    {
        // Arrange
        var task = new TaskEntity("Tarefa Teste", "Descrição", Guid.NewGuid());

        // Act
        var result = await _repository.AddAsync(task);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(task.Id);
        
        var savedTask = await _context.Tasks.FindAsync(task.Id);
        savedTask.Should().NotBeNull();
        savedTask!.Title.Should().Be("Tarefa Teste");
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_ExistingTask_ShouldReturnTask()
    {
        // Arrange
        var task = new TaskEntity("Tarefa Teste", "Descrição", Guid.NewGuid());
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(task.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Tarefa Teste");
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByIdAsync_NonExistingTask_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByUserIdAsync_ValidUserId_ShouldReturnUserTasks()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var task1 = new TaskEntity("Tarefa 1", "Descrição 1", userId);
        var task2 = new TaskEntity("Tarefa 2", "Descrição 2", userId);
        var task3 = new TaskEntity("Tarefa 3", "Descrição 3", Guid.NewGuid()); // Different user

        _context.Tasks.AddRange(task1, task2, task3);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByUserIdAsync(userId);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.UserId == userId);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetByStatusAsync_ValidStatus_ShouldReturnTasksWithStatus()
    {
        // Arrange
        var task1 = new TaskEntity("Tarefa 1", "Descrição 1", Guid.NewGuid());
        var task2 = new TaskEntity("Tarefa 2", "Descrição 2", Guid.NewGuid());
        task2.MarkAsCompleted();

        _context.Tasks.AddRange(task1, task2);
        await _context.SaveChangesAsync();

        // Act
        var pendingTasks = await _repository.GetByStatusAsync(TaskStatusEnum.Pending);
        var completedTasks = await _repository.GetByStatusAsync(TaskStatusEnum.Completed);

        // Assert
        pendingTasks.Should().HaveCount(1);
        pendingTasks.Should().OnlyContain(t => t.Status == TaskStatusEnum.Pending);

        completedTasks.Should().HaveCount(1);
        completedTasks.Should().OnlyContain(t => t.Status == TaskStatusEnum.Completed);
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteAsync_ExistingTask_ShouldMarkAsDeleted()
    {
        // Arrange
        var task = new TaskEntity("Tarefa Teste", "Descrição", Guid.NewGuid());
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(task);

        // Assert
        var deletedTask = await _context.Tasks.FindAsync(task.Id);
        deletedTask.Should().NotBeNull();
        deletedTask!.IsDeleted.Should().BeTrue();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
