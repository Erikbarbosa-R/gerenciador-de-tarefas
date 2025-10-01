using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Tasks.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Tasks.Queries.GetTask;

public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, Result<TaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTaskQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TaskDto>> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(request.Id, cancellationToken);
            if (task == null)
            {
                return Result.Failure<TaskDto>("Tarefa não encontrada");
            }

            var taskDto = new TaskDto
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
            };

            return Result.Success(taskDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<TaskDto>($"Erro ao buscar tarefa: {ex.Message}");
        }
    }
}
