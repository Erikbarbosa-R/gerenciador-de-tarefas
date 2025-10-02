using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Tests.Domain.Entities;

public class TaskTests
{
    [Fact]
    public void Constructor_ValidParameters_ShouldCreateTask()
    {
        // Arrange
        var title = "Tarefa Teste";
        var description = "Descrição da tarefa";
        var userId = Guid.NewGuid();
        var priority = TaskPriority.High;
        var dueDate = DateTime.UtcNow.AddDays(1);

        // Act
        var task = new TaskEntity(title, description, userId, priority, dueDate);

        // Assert
        task.Should().NotBeNull();
        task.Title.Should().Be(title);
        task.Description.Should().Be(description);
        task.UserId.Should().Be(userId);
        task.Priority.Should().Be(priority);
        task.DueDate.Should().Be(dueDate);
        task.Status.Should().Be(TaskStatusEnum.Pending);
        task.IsDeleted.Should().BeFalse();
        task.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateTitle_ValidTitle_ShouldUpdateTitle()
    {
        // Arrange
        var task = new TaskEntity("Título Original", "Descrição", Guid.NewGuid());
        var newTitle = "Novo Título";

        // Act
        task.UpdateTitle(newTitle);

        // Assert
        task.Title.Should().Be(newTitle);
        task.UpdatedAt.Should().NotBeNull();
        task.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void UpdateTitle_EmptyTitle_ShouldThrowException()
    {
        // Arrange
        var task = new TaskEntity("Título Original", "Descrição", Guid.NewGuid());

        // Act & Assert
        Assert.Throws<ArgumentException>(() => task.UpdateTitle(""));
        Assert.Throws<ArgumentException>(() => task.UpdateTitle("   "));
    }

    [Fact]
    public void ChangeStatus_ValidStatus_ShouldUpdateStatus()
    {
        // Arrange
        var task = new TaskEntity("Tarefa", "Descrição", Guid.NewGuid());

        // Act
        task.ChangeStatus(TaskStatusEnum.InProgress);

        // Assert
        task.Status.Should().Be(TaskStatusEnum.InProgress);
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void MarkAsCompleted_ShouldSetStatusToCompleted()
    {
        // Arrange
        var task = new TaskEntity("Tarefa", "Descrição", Guid.NewGuid());

        // Act
        task.MarkAsCompleted();

        // Assert
        task.Status.Should().Be(TaskStatusEnum.Completed);
        task.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void IsOverdue_TaskWithPastDueDate_ShouldReturnTrue()
    {
        // Arrange
        var task = new TaskEntity("Tarefa", "Descrição", Guid.NewGuid(), TaskPriority.Medium, DateTime.UtcNow.AddDays(-1));

        // Act & Assert
        task.IsOverdue().Should().BeTrue();
    }

    [Fact]
    public void IsOverdue_CompletedTask_ShouldReturnFalse()
    {
        // Arrange
        var task = new TaskEntity("Tarefa", "Descrição", Guid.NewGuid(), TaskPriority.Medium, DateTime.UtcNow.AddDays(-1));
        task.MarkAsCompleted();

        // Act & Assert
        task.IsOverdue().Should().BeFalse();
    }

    [Fact]
    public void IsCompleted_CompletedTask_ShouldReturnTrue()
    {
        // Arrange
        var task = new TaskEntity("Tarefa", "Descrição", Guid.NewGuid());
        task.MarkAsCompleted();

        // Act & Assert
        task.IsCompleted().Should().BeTrue();
    }
}
