using System;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.DTOs;

public record PatchTaskDto(
    string? Title = null,
    string? Description = null,
    TaskStatus? Status = null,
    TaskPriority? Priority = null,
    DateTime? DueDate = null,
    Guid? AssignedToUserId = null
);
