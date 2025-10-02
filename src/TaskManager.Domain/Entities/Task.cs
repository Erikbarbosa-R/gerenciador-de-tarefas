using TaskManager.Domain.Enums;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities;

public class Task : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Enums.TaskStatus Status { get; private set; } = Enums.TaskStatus.Pending;
    public TaskPriority Priority { get; private set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;
    public Guid? AssignedToUserId { get; private set; }
    public User? AssignedToUser { get; private set; }

    private Task() { }

    public Task(string title, string description, Guid userId, TaskPriority priority = TaskPriority.Medium, DateTime? dueDate = null, Guid? assignedToUserId = null)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        UserId = userId;
        Priority = priority;
        DueDate = dueDate;
        AssignedToUserId = assignedToUserId;
    }

    public void UpdateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Título não pode ser vazio", nameof(title));

        Title = title;
        MarkAsUpdated();
    }

    public void UpdateDescription(string description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
        MarkAsUpdated();
    }

    public void ChangeStatus(Enums.TaskStatus status)
    {
        Status = status;
        MarkAsUpdated();
    }

    public void ChangePriority(TaskPriority priority)
    {
        Priority = priority;
        MarkAsUpdated();
    }

    public void SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        MarkAsUpdated();
    }

    public void MarkAsCompleted()
    {
        Status = Enums.TaskStatus.Completed;
        MarkAsUpdated();
    }

    public void MarkAsInProgress()
    {
        Status = Enums.TaskStatus.InProgress;
        MarkAsUpdated();
    }

    public void MarkAsPending()
    {
        Status = Enums.TaskStatus.Pending;
        MarkAsUpdated();
    }

    public bool IsOverdue()
    {
        return DueDate.HasValue && DueDate.Value < DateTime.UtcNow && Status != Enums.TaskStatus.Completed;
    }

    public bool IsCompleted()
    {
        return Status == Enums.TaskStatus.Completed;
    }

    public bool IsInProgress()
    {
        return Status == Enums.TaskStatus.InProgress;
    }

    public bool IsPending()
    {
        return Status == Enums.TaskStatus.Pending;
    }

    public void AssignToUser(Guid? assignedToUserId)
    {
        AssignedToUserId = assignedToUserId;
        MarkAsUpdated();
    }

    public void UnassignUser()
    {
        AssignedToUserId = null;
        MarkAsUpdated();
    }

    public bool IsAssigned()
    {
        return AssignedToUserId.HasValue;
    }

    public bool IsAssignedToUser(Guid userId)
    {
        return AssignedToUserId == userId;
    }
}
