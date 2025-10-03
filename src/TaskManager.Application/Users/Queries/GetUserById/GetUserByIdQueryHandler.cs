using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Users.DTOs;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id, cancellationToken);
            
            if (user == null || !user.IsActive || user.IsDeleted)
            {
                return Result.Failure<UserDto>("Usuário não encontrado");
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email.Value,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

            return Result.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<UserDto>($"Erro ao buscar usuário: {ex.Message}");
        }
    }
}
