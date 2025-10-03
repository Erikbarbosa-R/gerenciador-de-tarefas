using System;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(
    Guid Id,
    string? Title = null,
    string? Description = null,
    TaskStatus? Status = null,
    TaskPriority? Priority = null,
    DateTime? DueDate = null,
    Guid? AssignedToUserId = null,
    bool? RemoveAssignment = null
) : IRequest<Result>;
