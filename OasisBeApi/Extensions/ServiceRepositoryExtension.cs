using Oasis.Repositories;
using Oasis.Repositories.Interfaces;
using Oasis.Services;
using Oasis.Services.Interfaces;

namespace Oasis.Extensions;

public static class ServiceRepositoryExtension {
    public static IServiceCollection AddApplicationServices(this IServiceCollection services) {
        // Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<ITeamRepository, TeamRepository>();
        services.AddScoped<ITeamInvitationRepository, TeamInvitationRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<ILevelRepository, LevelRepository>();

        // Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<ITeamInvitationService, TeamInvitationService>();
        services.AddScoped<IMemberService, MemberService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IGeminiService, GeminiService>();
        services.AddScoped<ILevelService, LevelService>();

        return services;
    }
}