using TaskManager.Domain.ValueObjects;

namespace TaskManager.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public PasswordHash PasswordHash { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;

    private readonly List<Entities.Task> _tasks = new();
    public IReadOnlyCollection<Entities.Task> Tasks => _tasks.AsReadOnly();

    private User() { }

    public User(string name, Email email, PasswordHash passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome não pode ser vazio", nameof(name));

        Name = name;
        MarkAsUpdated();
    }

    public void UpdateEmail(Email email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        MarkAsUpdated();
    }

    public void UpdatePassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        MarkAsUpdated();
    }


    public void Activate()
    {
        IsActive = true;
        MarkAsUpdated();
    }

    public void Deactivate()
    {
        IsActive = false;
        MarkAsUpdated();
    }

    public void AddTask(Entities.Task task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        _tasks.Add(task);
    }

    public void RemoveTask(Entities.Task task)
    {
        if (task == null)
            throw new ArgumentNullException(nameof(task));

        _tasks.Remove(task);
    }
}
