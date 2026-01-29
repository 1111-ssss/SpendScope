using admin.AppBootstrapper.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace admin.AppBootstrapper;

public static class ApplicationBootstrapper
{
    public static IHost CreateHost()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .CreateLogger();

        return Host.CreateDefaultBuilder()
            .UseSerilog()
            .ConfigureAppConfiguration(c =>
            {
                var basePath = Path.GetDirectoryName(AppContext.BaseDirectory)
                    ?? throw new DirectoryNotFoundException("Директория приложения не найдена.");
                c.SetBasePath(basePath);
            })
            .ConfigureServices(services =>
            {
                services.AddShellServices();

                services.AddAppServices();

                services.AddApiServices();

                services.AddVMsAndPages();
            })
            .Build();
    }
}
