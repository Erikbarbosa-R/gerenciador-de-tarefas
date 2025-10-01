using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using System.Threading.Tasks;

namespace TaskManager.Domain.Interfaces;

public interface ITaskRepository : IRepository<Entities.Task>
{
    System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetByStatusAsync(Enums.TaskStatus status, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetByPriorityAsync(TaskPriority priority, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetOverdueTasksAsync(CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetTasksDueTodayAsync(CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<IEnumerable<Entities.Task>> SearchTasksAsync(string searchTerm, CancellationToken cancellationToken = default);
}
