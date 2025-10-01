namespace TaskManager.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Configurações específicas da API podem ser adicionadas aqui
        return services;
    }
}
