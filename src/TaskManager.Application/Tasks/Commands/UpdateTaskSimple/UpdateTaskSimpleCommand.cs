using System;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands.UpdateTaskSimple;

public record UpdateTaskSimpleCommand(
    Guid TaskId,
    string Title,
    string Description,
    TaskStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    Guid? AssignedToUserId = null
) : IRequest<Result>;
