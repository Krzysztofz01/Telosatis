namespace Telosatis.Infrastructure.Database;

internal static class Constants
{
    public const int Timeout = 30;

    private const string SqliteDatabaseName = "database.sqlite";
    
    public static readonly string SqliteDatabaseProductionPath =
        Path.Join("/var/lib/telosatis", SqliteDatabaseName);

    public static readonly string SqliteDatabaseDevelopmentPath =
        Path.Join(AppDomain.CurrentDomain.BaseDirectory, SqliteDatabaseName);
    
    public static string SqliteConnectionString(string sqliteDatabasePath) =>
        $"Data Source={sqliteDatabasePath};Cache=Shared;Foreign Keys=True;Default Timeout={Timeout};";
}