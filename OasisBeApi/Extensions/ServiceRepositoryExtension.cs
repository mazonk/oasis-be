using Oasis.Repositories;
using Oasis.Repositories.Interfaces;
using Oasis.Services;
using Oasis.Services.Interfaces;

namespace Oasis.Extensions;

public static class ServiceRepositoryExtension {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        // Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();

        // Services
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}