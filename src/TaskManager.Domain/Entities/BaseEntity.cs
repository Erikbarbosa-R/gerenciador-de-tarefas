using TaskManager.Domain.Helpers;

namespace TaskManager.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; protected set; } = DateTimeHelper.Now;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;

    protected BaseEntity() { }

    protected BaseEntity(Guid id)
    {
        Id = id;
    }

    public void MarkAsUpdated()
    {
        UpdatedAt = DateTimeHelper.Now;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTimeHelper.Now;
    }
}
