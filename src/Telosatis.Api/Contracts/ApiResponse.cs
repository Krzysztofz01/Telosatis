namespace Telosatis.Api.Contracts;

public sealed record ApiResponse<T>(
    int StatusCode,
    T? Data,
    Error? Error);
    
public sealed record Error(
    string Id,
    string Description);