using System;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TaskManager.Application.Common.Models;
using TaskManager.Application.Users.DTOs;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.ValueObjects;

namespace TaskManager.Application.Users.Commands.AuthenticateUser;

public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, Result<AuthenticationResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthenticateUserCommandHandler(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<Result<AuthenticationResultDto>> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var email = new Email(request.Email);
            var user = await _unitOfWork.Users.GetByEmailAsync(email, cancellationToken);

            if (user == null || !user.IsActive)
            {
                return Result.Failure<AuthenticationResultDto>("Credenciais inválidas");
            }

            if (!user.PasswordHash.Verify(request.Password))
            {
                return Result.Failure<AuthenticationResultDto>("Credenciais inválidas");
            }

            var token = GenerateJwtToken(user);

            var result = new AuthenticationResultDto
            {
                Token = token,
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email.Value,
                Role = user.Role
            };

            return Result.Success(result);
        }
        catch (Exception ex)
        {
            return Result.Failure<AuthenticationResultDto>($"Erro na autenticação: {ex.Message}");
        }
    }

    private string GenerateJwtToken(Domain.Entities.User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
