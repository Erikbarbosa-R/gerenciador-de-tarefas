using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TaskManager.Application.Common.Models;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var email = new Email(request.Email);
            
            var emailExists = await _unitOfWork.Users.EmailExistsAsync(email, cancellationToken);
            if (emailExists)
            {
                return Result.Failure<Guid>("Email já está em uso");
            }

            var passwordHash = PasswordHash.Create(request.Password);
            var user = new User(request.Name, email, passwordHash, request.Role);

            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>($"Erro ao criar usuário: {ex.Message}");
        }
    }
}
