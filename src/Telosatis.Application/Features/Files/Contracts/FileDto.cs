namespace Telosatis.Application.Features.Files.Contracts;

public sealed record FileDto(
    FileInfoDto Info,
    Stream Content) : IDisposable, IAsyncDisposable
{
    public void Dispose() => Content.Dispose();
    public ValueTask DisposeAsync() => Content.DisposeAsync();
}