using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            // Primeiro tenta pegar do DATABASE_URL (Railway)
            var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            
            // Se não encontrar, usa a string de conexão do appsettings
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }
            
            // Verificar se a string de conexão está vazia ou nula
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("String de conexão não configurada. Verifique a variável DATABASE_URL ou DefaultConnection.");
            }
            
            // Log da string de conexão para debug (sem senha)
            Console.WriteLine($"DEBUG: Connection string = {connectionString.Replace("Password=WASkPWMvXQiIMkMjoXxpzCWTzLwOyVwi", "Password=***")}");
            
            // Se for uma URL de conexão PostgreSQL (Railway), converter para formato Npgsql
            if (connectionString.StartsWith("postgresql://"))
            {
                // Converter URL para formato de string de conexão
                var uri = new Uri(connectionString);
                var host = uri.Host;
                var port = uri.Port;
                var database = uri.AbsolutePath.TrimStart('/');
                var username = uri.UserInfo.Split(':')[0];
                var password = uri.UserInfo.Split(':')[1];
                
                var npgsqlConnectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};";
                
                options.UseNpgsql(
                    npgsqlConnectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            }
            else
            {
                // String de conexão PostgreSQL tradicional
                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            }
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
