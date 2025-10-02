using System;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.DTOs;

public record UpdateTaskWithoutIdDto(
    string Title,
    string Description,
    TaskStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    Guid? AssignedToUserId = null
);
