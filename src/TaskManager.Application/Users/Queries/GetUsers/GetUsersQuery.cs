using System.Collections.Generic;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Users.DTOs;

namespace TaskManager.Application.Users.Queries.GetUsers;

public record GetUsersQuery() : IRequest<Result<List<UserDto>>>;
