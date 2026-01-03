using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DI
{
    public static class InitStorageExtensions
    {
        public static IApplicationBuilder InitStorage(this IApplicationBuilder app, IConfiguration configuration)
        {
            //Init directories
            var directories = new[]
            {
                configuration["AppStorage:ApkPath"] ?? "ApkStorage",
                configuration["AppStorage:AvatarPath"] ?? "AvatarStorage",
                configuration["AppStorage:AchievementsPath"] ?? "AchievementsStorage"
            };

            foreach (var path in directories)
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), path);
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
            }

            return app;
        }
    }
}