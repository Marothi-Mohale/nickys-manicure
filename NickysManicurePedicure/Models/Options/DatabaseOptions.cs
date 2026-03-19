using System.ComponentModel.DataAnnotations;

namespace NickysManicurePedicure.Models.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    [Required]
    [RegularExpression("(?i)^(sqlite|postgresql)$", ErrorMessage = "Database provider must be either Sqlite or PostgreSql.")]
    public string Provider { get; set; } = DatabaseProviders.Sqlite;

    public bool ApplyMigrationsOnStartup { get; set; }

    public bool SeedOnStartup { get; set; } = true;
}

public static class DatabaseProviders
{
    public const string Sqlite = "sqlite";
    public const string PostgreSql = "postgresql";
}
