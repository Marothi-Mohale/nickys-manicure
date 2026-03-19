using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.Data;

public static class DatabaseProviderConfigurator
{
    public static void Configure(
        DbContextOptionsBuilder options,
        string provider,
        string connectionString,
        string contentRootPath)
    {
        switch (provider)
        {
            case DatabaseProviders.Sqlite:
                EnsureSqliteDataDirectoryExists(contentRootPath, connectionString);
                options.UseSqlite(connectionString);
                break;
            case DatabaseProviders.PostgreSql:
                options.UseNpgsql(connectionString, npgsql => npgsql.EnableRetryOnFailure());
                break;
            default:
                throw new InvalidOperationException(
                    $"Unsupported database provider '{provider}'. Supported values: Sqlite, PostgreSql.");
        }
    }

    private static void EnsureSqliteDataDirectoryExists(string contentRootPath, string connectionString)
    {
        SqliteConnectionStringBuilder builder;

        try
        {
            builder = new SqliteConnectionStringBuilder(connectionString);
        }
        catch (ArgumentException)
        {
            return;
        }

        var databasePath = builder.DataSource?.Trim();

        if (string.IsNullOrWhiteSpace(databasePath) || databasePath == ":memory:")
        {
            return;
        }

        var fullPath = Path.IsPathRooted(databasePath)
            ? databasePath
            : Path.GetFullPath(databasePath, contentRootPath);
        var directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
