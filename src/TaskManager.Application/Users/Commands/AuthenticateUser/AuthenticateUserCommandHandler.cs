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
using TaskManager.Domain.Helpers;

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
                Email = user.Email.Value
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
        var jwtKeyString = _configuration["Jwt:Key"]!;
        
        // Log para debug da JWT Key
        Console.WriteLine($"DEBUG JWT Key Length: {jwtKeyString.Length} characters");
        Console.WriteLine($"DEBUG JWT Key Bytes Length: {Encoding.UTF8.GetBytes(jwtKeyString).Length} bytes");
        Console.WriteLine($"DEBUG JWT Key Bits: {Encoding.UTF8.GetBytes(jwtKeyString).Length * 8} bits");
        Console.WriteLine($"DEBUG JWT Key (first 20 chars): {jwtKeyString.Substring(0, Math.Min(20, jwtKeyString.Length))}...");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKeyString));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim(ClaimTypes.Role, "User")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTimeHelper.Now.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
