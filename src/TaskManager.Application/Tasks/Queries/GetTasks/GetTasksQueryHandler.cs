using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Tasks.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, Result<IEnumerable<TaskDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTasksQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<TaskDto>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IEnumerable<Domain.Entities.Task> tasks;

            if (request.UserId.HasValue)
            {
                tasks = await _unitOfWork.Tasks.GetByUserIdAsync(request.UserId.Value, cancellationToken);
            }
            else if (request.Status.HasValue)
            {
                tasks = await _unitOfWork.Tasks.GetByStatusAsync(request.Status.Value, cancellationToken);
            }
            else if (request.Priority.HasValue)
            {
                tasks = await _unitOfWork.Tasks.GetByPriorityAsync(request.Priority.Value, cancellationToken);
            }
            else if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                tasks = await _unitOfWork.Tasks.SearchTasksAsync(request.SearchTerm, cancellationToken);
            }
            else
            {
                tasks = await _unitOfWork.Tasks.GetAllAsync(cancellationToken);
            }

            var taskDtos = tasks.Select(task => new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Priority = task.Priority,
                DueDate = task.DueDate,
                UserId = task.UserId,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            });

            return Result.Success(taskDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<TaskDto>>($"Erro ao buscar tarefas: {ex.Message}");
        }
    }
}
