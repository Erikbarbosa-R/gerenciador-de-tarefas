using System;
using System.Collections.Generic;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Tasks.DTOs;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tasks.Queries.GetTasks;

public record GetTasksQuery(
    Guid? UserId = null,
    TaskStatus? Status = null,
    TaskPriority? Priority = null,
    string? SearchTerm = null
) : IRequest<Result<IEnumerable<TaskDto>>>;
