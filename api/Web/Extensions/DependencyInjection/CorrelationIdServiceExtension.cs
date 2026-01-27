using CorrelationId.DependencyInjection;

namespace Web.Extensions.DependencyInjection;

public static class CorrelationIdServiceExtension
{
    public static IServiceCollection AddCorrelationId(this IServiceCollection services)
    {
        services.AddDefaultCorrelationId(conf =>
        {
            conf.IgnoreRequestHeader = true;
            conf.UpdateTraceIdentifier = true;
        });

        return services;
    }
}