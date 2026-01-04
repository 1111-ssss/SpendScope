using Application.Abstractions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Storage
{
    public class FileStorage : IFileStorage
    {
        private readonly string _basePath;
        private readonly ILogger<FileStorage> _logger;
        public FileStorage(IConfiguration config, ILogger<FileStorage> logger)
        {
            _logger = logger;
            _basePath = config["AppStorage:BasePath"] ?? throw new ArgumentNullException("BasePath не указан");
            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }
        public async Task<string> SaveFileAsync(IFormFile file, string subDirectory, string fileName, CancellationToken ct = default)
        {
            var directory = Path.Combine(_basePath, subDirectory);
            Directory.CreateDirectory(directory);

            var fullPath = Path.Combine(directory, fileName);

            await using var stream = File.Create(fullPath);
            _logger.LogInformation($"Сохранение файла в папку {directory}");
            await file.CopyToAsync(stream, ct);

            return Path.Combine(subDirectory, fileName).Replace("\\", "/");
        }
        public async Task DeleteFileAsync(string filePath, CancellationToken ct = default)
        {
            var fullPath = Path.Combine(_basePath, filePath);
            if (!File.Exists(fullPath))
                return;
            _logger.LogInformation($"Удаление файла {fullPath}");
            File.Delete(fullPath);
        }
        public string? GetFilePath(string? relativePath, string? defaultDirectory = null)
        {
            if (string.IsNullOrEmpty(relativePath) && string.IsNullOrEmpty(defaultDirectory))
                return null;

            if (!string.IsNullOrEmpty(relativePath))
            {
                string fullPath = Path.Combine(_basePath, relativePath);
                if (File.Exists(fullPath))
                    return fullPath;
            }

            if (!string.IsNullOrEmpty(defaultDirectory))
            {
                string fallbackPath = Path.Combine(_basePath, defaultDirectory);
                if (File.Exists(fallbackPath))
                    return fallbackPath;
            }

            return null;
        }
    }
}