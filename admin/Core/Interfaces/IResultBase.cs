using admin.Core.Enums;

namespace admin.Core.Interfaces;

public interface IResultBase
{
    bool IsSuccess { get; }
    ResultErrorCode? Error { get; }
    string? Message { get; }
}
