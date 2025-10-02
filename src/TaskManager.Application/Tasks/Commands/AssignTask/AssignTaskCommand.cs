using System;
using MediatR;
using TaskManager.Application.Common.Models;

namespace TaskManager.Application.Tasks.Commands.AssignTask;

public record AssignTaskCommand(
    Guid TaskId,
    Guid? AssignedToUserId
) : IRequest<Result>;
