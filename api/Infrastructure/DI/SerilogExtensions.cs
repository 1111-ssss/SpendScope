using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infrastructure.DI;

public static class SerilogExtensions
{
    public static IHostBuilder UseCustomSerilog(this IHostBuilder hostBuilder)
    {
        return hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .WriteTo.Console()
                .WriteTo.Async(a => a.PostgreSQL(
                    connectionString: context.Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Logs",
                    needAutoCreateTable: true
                ));
        });
    }
}