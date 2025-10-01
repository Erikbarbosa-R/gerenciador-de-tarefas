using System;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Users.Commands.CreateUser;

public record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    UserRole Role = UserRole.User
) : IRequest<Result<Guid>>;
