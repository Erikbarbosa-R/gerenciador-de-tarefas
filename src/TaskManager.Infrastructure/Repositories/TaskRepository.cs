using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly IApplicationDbContext _context;

    public TaskRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Task?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Include(t => t.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Task> AddAsync(Domain.Entities.Task entity, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async System.Threading.Tasks.Task UpdateAsync(Domain.Entities.Task entity, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async System.Threading.Tasks.Task DeleteAsync(Domain.Entities.Task entity, CancellationToken cancellationToken = default)
    {
        entity.MarkAsDeleted();
        _context.Tasks.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Include(t => t.User)
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetByStatusAsync(Domain.Enums.TaskStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Include(t => t.User)
            .Where(t => t.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetByPriorityAsync(TaskPriority priority, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Include(t => t.User)
            .Where(t => t.Priority == priority)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetOverdueTasksAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.Tasks
            .Include(t => t.User)
            .Where(t => t.DueDate.HasValue && t.DueDate.Value < now && t.Status != Domain.Enums.TaskStatus.Completed)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> GetTasksDueTodayAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);
        
        return await _context.Tasks
            .Include(t => t.User)
            .Where(t => t.DueDate.HasValue && t.DueDate.Value >= today && t.DueDate.Value < tomorrow)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Domain.Entities.Task>> SearchTasksAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Include(t => t.User)
            .Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm))
            .ToListAsync(cancellationToken);
    }
}
