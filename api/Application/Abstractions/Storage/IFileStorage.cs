using Domain.Abstractions.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Abstractions.Storage
{
    public interface IFileStorage
    {
        Task<string> SaveFileAsync(IFormFile file, string subDirectory, CancellationToken ct = default);
        Task DeleteFileAsync(string filePath, CancellationToken ct = default);
        string? GetFilePath(string relativePath);
    }
}