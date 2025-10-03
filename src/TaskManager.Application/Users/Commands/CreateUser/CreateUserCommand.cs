using System;
using MediatR;
using TaskManager.Application.Common.Models;

namespace TaskManager.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password
) : IRequest<Result<Guid>>;
