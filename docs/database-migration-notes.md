# Database Migration Notes

The current backend runs on EF Core with SQLite, but the data layer is shaped to keep a later move to PostgreSQL or SQL Server straightforward.

## What is already migration-friendly

- Business logic uses `ApplicationDbContext` directly and does not branch on provider behavior.
- Entity configuration is centralized under `NickysManicurePedicure/Data/Configurations`.
- Provider selection is isolated in `NickysManicurePedicure/Data/DatabaseProviderConfigurator.cs`.
- Audit timestamps are normalized to UTC in `NickysManicurePedicure/Data/ApplicationDbContext.cs`.
- Check constraint SQL is centralized in `NickysManicurePedicure/Data/ConfigurationSql.cs` instead of being scattered through entity configurations.

## What would need to change during migration

1. Add the target EF Core provider package if it is not already referenced.
2. Add a real migration history:
   - create the initial EF Core migration
   - stop relying on `EnsureCreated`
   - use migrations for all schema evolution
3. Update configuration:
   - add the target connection string
   - set `Database:Provider` to the new provider
4. Review indexes and constraints on the target engine after generating migrations:
   - precision and scale on decimals
   - check constraints
   - string column lengths and collation expectations
5. Review search/filter semantics:
   - SQLite string comparisons can behave differently from PostgreSQL and SQL Server
   - if case-insensitive search becomes a hard requirement, prefer an explicit normalized column strategy or provider-aware collation decisions during migration
6. Validate date/time round-tripping end to end with the new provider.

## Current known limitations

- No EF Core migrations are checked into the project yet.
- Gallery category filtering currently relies on translated lowercase comparison. It works today, but if stricter portability or performance is needed later, introduce a normalized category key column rather than provider-specific query hacks.

## Recommended migration order

1. Create the first SQLite migration from the current model.
2. Apply it to a clean local database and verify startup/seed behavior.
3. Add the new provider and generate provider-appropriate migrations.
4. Run integration tests against the target provider before switching environments.
