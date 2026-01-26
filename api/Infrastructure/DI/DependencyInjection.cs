using Application.Abstractions.Auth;
using Application.Abstractions.DataBase;
using Application.Abstractions.Misc;
using Application.Abstractions.Repository;
using Application.Abstractions.Storage;
using Infrastructure.DataBase.Context;
using Infrastructure.DataBase.Repository;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Misc;
using Infrastructure.Services.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DI;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //Database
        services.AddDataAccess(configuration);

        //Auth
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        //Storage services
        services.AddScoped<IFileStorage, FileStorage>();
        services.AddScoped<IImageFormatter, ImageFormatter>();

        //Misc
        services.AddSingleton<IRequestStatisticsService, RequestStatisticsService>();
        services.AddSingleton<ICpuUsageService, CpuUsageService>();

        return services;
    }
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());
        //services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        return services;
    }
}