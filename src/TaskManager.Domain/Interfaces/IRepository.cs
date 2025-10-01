using TaskManager.Domain.Entities;
using System.Threading.Tasks;

namespace TaskManager.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    System.Threading.Tasks.Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
