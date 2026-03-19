using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Data;

namespace NickysManicurePedicure.Tests.Infrastructure;

public sealed class TestSqliteDatabase : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    public TestSqliteDatabase()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        using var context = CreateDbContext();
        context.Database.EnsureCreated();
    }

    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        return new ApplicationDbContext(options);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}
