using Infrastructure.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Application.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddScoped<IUnitOfWork, AppDbContext>();

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

        return services;
    }
}