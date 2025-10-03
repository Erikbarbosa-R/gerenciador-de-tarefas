using System;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Users.DTOs;

namespace TaskManager.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;
