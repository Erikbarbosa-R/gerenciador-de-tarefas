using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Users.DTOs;

namespace TaskManager.Application.Users.Commands.AuthenticateUser;

public record AuthenticateUserCommand(
    string Email,
    string Password
) : IRequest<Result<AuthenticationResultDto>>;
