using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Users.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUsersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _unitOfWork.Users.GetActiveUsersAsync(cancellationToken);
            
            var userDtos = users
                .Where(u => !u.IsDeleted)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email.Value,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                })
                .ToList();

            return Result.Success(userDtos);
        }
        catch (Exception ex)
        {
            return Result.Failure<List<UserDto>>($"Erro ao buscar usuários: {ex.Message}");
        }
    }
}
