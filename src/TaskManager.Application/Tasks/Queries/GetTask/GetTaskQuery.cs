using System;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Tasks.DTOs;

namespace TaskManager.Application.Tasks.Queries.GetTask;

public record GetTaskQuery(Guid Id) : IRequest<Result<TaskDto>>;
