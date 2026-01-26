using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Storage;

public interface IFileStorage
{
    long GetStorageSize(string? subDirectory);
    Task<string> SaveFileAsync(IFormFile file, string subDirectory, string fileName, CancellationToken ct = default);
    Task DeleteFileAsync(string filePath, CancellationToken ct = default);
    string? GetFilePath(string? relativePath, string? defaultDirectory = null);
}