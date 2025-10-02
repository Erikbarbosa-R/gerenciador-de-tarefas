using System;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string Description,
    Guid UserId,
    TaskPriority Priority = TaskPriority.Medium,
    DateTime? DueDate = null,
    Guid? AssignedToUserId = null
) : IRequest<Result<Guid>>;
