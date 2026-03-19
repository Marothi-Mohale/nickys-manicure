# Nicky's Manicure & Pedicure Backend Architecture

## Concise Architecture Summary

Build the backend as a modular monolith in a single ASP.NET Core Web API application on .NET 8, using EF Core with SQLite in the current phase and provider-agnostic persistence rules so PostgreSQL can replace SQLite later with minimal application-layer impact.

Organize the solution around clear layers:

- `Domain`: entities, enums, domain rules, shared base types
- `Application`: use cases, DTO contracts, validators, mapping, service interfaces
- `Infrastructure`: EF Core, logging, exception handling, provider wiring, seeders
- `Api`: controllers, request/response contracts, filters, OpenAPI
- `Tests`: unit, integration, persistence, migration tests

For this codebase, the practical target is to keep one deployable web project but structure folders by these layers internally. That preserves the current simplicity while reducing coupling between controllers, EF, and DTOs.

## Recommended Project Structure

Current repo already has a good start with `Api`, `Application`, `Contracts`, `Data`, `Infrastructure`, and tests. The recommended target is:

```text
NickysManicurePedicure/
  Api/
    Controllers/
      Public/
        BusinessProfileController.cs
        ServicesController.cs
        GalleryController.cs
        FaqController.cs
        TestimonialsController.cs
        InquiriesController.cs
      Admin/
        ServicesAdminController.cs
        GalleryAdminController.cs
        FaqAdminController.cs
        TestimonialsAdminController.cs
        BusinessProfileAdminController.cs
        InquiriesAdminController.cs
    Contracts/
      Requests/
      Responses/
      Common/
    Filters/
      CorrelationIdFilter.cs
      RequestLoggingFilter.cs
    Extensions/
      ApiServiceCollectionExtensions.cs
  Application/
    Abstractions/
      Services/
      Repositories/
      Clock/
    UseCases/
      BusinessProfile/
      Services/
      Gallery/
      Testimonials/
      Faq/
      Inquiries/
    DTOs/
      Requests/
      Responses/
      Internal/
    Validators/
    Mapping/
    Results/
    Behaviors/
  Domain/
    Entities/
      BusinessProfile.cs
      BusinessHour.cs
      SalonService.cs
      GalleryItem.cs
      Testimonial.cs
      FaqItem.cs
      Inquiry.cs
    Enums/
      InquiryStatus.cs
      InquiryType.cs
    ValueObjects/
      MoneyText.cs
      ContactDetails.cs
      BusinessAddress.cs
    Common/
      BaseEntity.cs
      AuditableEntity.cs
  Infrastructure/
    Persistence/
      ApplicationDbContext.cs
      Configurations/
        BusinessProfileConfiguration.cs
        BusinessHourConfiguration.cs
        SalonServiceConfiguration.cs
        GalleryItemConfiguration.cs
        TestimonialConfiguration.cs
        FaqItemConfiguration.cs
        InquiryConfiguration.cs
      Migrations/
        Sqlite/
        PostgreSql/
      Seed/
        SeedRunner.cs
        StaticContentSeed.cs
        BusinessProfileSeed.cs
        InquirySeed.cs
      Interceptors/
        AuditSaveChangesInterceptor.cs
    Logging/
      LogEventIds.cs
      LoggingScopes.cs
    Exceptions/
      DomainValidationException.cs
      NotFoundException.cs
      ConflictException.cs
    Configuration/
      DatabaseOptions.cs
      BusinessProfileOptions.cs
    DependencyInjection/
      InfrastructureServiceCollectionExtensions.cs
  Shared/
    Constants/
    Extensions/
  Program.cs

NickysManicurePedicure.Tests/
  Unit/
    Application/
    Domain/
  Integration/
    Api/
    Persistence/
  Architecture/
  Fixtures/
```

## 1. Entity Model

### Core aggregates

- `BusinessProfile`
  - Single-record aggregate for salon identity and contact details
  - Owns many `BusinessHour`
- `SalonService`
  - Public service catalog item
  - Admin-managed content
- `GalleryItem`
  - Published/unpublished portfolio content
- `Testimonial`
  - Published client review content
- `FaqItem`
  - Public FAQ content
- `Inquiry`
  - Operational aggregate for both contact and booking submissions

### Recommended entity refinements

- Add `CreatedUtc` and `UpdatedUtc` to all mutable content entities, not only some of them.
- Keep `Inquiry` as one table, but model type-specific fields carefully:
  - `InquiryType`
  - `PreferredService`
  - `PreferredDate`
  - `PreferredTime`
- Prefer `decimal? EstimatedPrice` in future admin/business flows; keep public-facing `PriceFromText` for display if pricing remains non-normalized.
- Consider `IsActive` or `IsPublished` consistently for all public content tables.
- Add optimistic concurrency token later when admin editing is introduced:
  - SQLite: application-managed `UpdatedUtc`
  - PostgreSQL: can later move to `xmin` or keep app-managed concurrency

### Suggested base types

- `BaseEntity`
  - `Id`
- `AuditableEntity : BaseEntity`
  - `CreatedUtc`
  - `UpdatedUtc`

### Suggested future entity snapshot

- `BusinessProfile`
  - `Id`, `Name`, `Tagline`, `Phone`, `PhoneHref`, `Email`
  - `AddressLine1`, `Suburb`, `City`, `Region`, `PostalCode`
  - `WhatsAppHref`, `InstagramHandle`, `BookingPolicy`, `AboutSummary`
  - `CreatedUtc`, `UpdatedUtc`
- `BusinessHour`
  - `Id`, `BusinessProfileId`, `DayOfWeek`, `IsClosed`, `OpenTime`, `CloseTime`, `Notes`, `DisplayOrder`
- `SalonService`
  - `Id`, `Name`, `Description`, `DurationText`, `PriceFromText`, `IsFeatured`, `IsPublished`, `DisplayOrder`, `CreatedUtc`, `UpdatedUtc`
- `GalleryItem`
  - `Id`, `Title`, `Description`, `Category`, `ImageUrl`, `ThumbnailUrl`, `AltText`, `IsFeatured`, `IsPublished`, `DisplayOrder`, `CreatedUtc`, `UpdatedUtc`
- `Testimonial`
  - `Id`, `ClientName`, `Highlight`, `Review`, `IsPublished`, `DisplayOrder`, `CreatedUtc`, `UpdatedUtc`
- `FaqItem`
  - `Id`, `Question`, `Answer`, `IsPublished`, `DisplayOrder`, `CreatedUtc`, `UpdatedUtc`
- `Inquiry`
  - `Id`, `InquiryType`, `Status`, `FullName`, `Email`, `PhoneNumber`, `PreferredService`, `PreferredDate`, `PreferredTime`, `Message`, `SourcePage`, `CreatedUtc`, `UpdatedUtc`

## 2. DbContext Design

Use one `ApplicationDbContext` for the current modular monolith.

### DbSets

- `BusinessProfiles`
- `BusinessHours`
- `SalonServices`
- `GalleryItems`
- `Testimonials`
- `FaqItems`
- `Inquiries`

### Configuration approach

Move all fluent configuration out of `OnModelCreating` into `IEntityTypeConfiguration<T>` classes. `ApplicationDbContext` should only do:

- `ApplyConfigurationsFromAssembly(...)`
- provider-agnostic global conventions
- optional audit/concurrency hooks

### Recommended conventions

- All tables use snake_case only if planned from day one, otherwise preserve current casing until PostgreSQL migration cutover to reduce churn.
- Explicit max lengths for all strings.
- Enum-to-string conversion for readability on both SQLite and PostgreSQL.
- Explicit indexes for query paths:
  - `Inquiry.CreatedUtc`
  - `Inquiry.Status, InquiryType, CreatedUtc`
  - `GalleryItem.IsPublished, DisplayOrder`
  - `GalleryItem.Category`
  - `SalonService.IsPublished, IsFeatured, DisplayOrder`
  - `FaqItem.IsPublished, DisplayOrder`
  - `Testimonial.IsPublished, DisplayOrder`
  - unique `BusinessHour(BusinessProfileId, DayOfWeek)`

### SaveChanges behavior

Use a `SaveChangesInterceptor` or overridden `SaveChangesAsync` to:

- stamp `CreatedUtc` and `UpdatedUtc`
- normalize trimmed strings where appropriate
- optionally enforce invariant checks before persistence

## 3. DTO Strategy

Keep DTOs separate from entities and split by API boundary.

### Rules

- Request DTOs for external input only
- Response DTOs for external output only
- Internal application models for service-to-service communication if needed
- Never expose EF entities directly from controllers

### Recommended grouping

- `Api.Contracts.Requests`
  - `CreateBookingRequestRequest`
  - `CreateContactInquiryRequest`
  - `GetServicesQuery`
  - `GetGalleryQuery`
- `Api.Contracts.Responses`
  - `BusinessProfileResponse`
  - `SalonServiceResponse`
  - `InquiryAcceptedResponse`
- `Application.DTOs`
  - only when a use case needs input/output models independent of HTTP

### DTO versioning

- Start with unversioned `/api/...`
- Make contracts additive where possible
- If breaking changes appear later, add `/api/v2`

## 4. Validation Strategy

Use layered validation, not just DataAnnotations.

### Layer 1: transport validation

Use DataAnnotations on simple HTTP DTO rules:

- required fields
- max lengths
- email/phone format
- pagination bounds

### Layer 2: business validation

Add dedicated validators or domain/application validation classes for rules like:

- `PreferredDate` cannot be in the past
- `PreferredTime` required for booking inquiry
- published item must have image URL for gallery
- `OpenTime < CloseTime` when not closed

### Layer 3: persistence validation

Enforce final integrity with EF config and database constraints:

- not null
- max length
- unique indexes
- foreign keys

### Error response shape

Standardize on RFC 7807 `ProblemDetails` and `ValidationProblemDetails` for:

- request validation failures
- not found
- conflicts
- unexpected errors

## 5. Service Layer Design

The service layer should separate read and write use cases.

### Public read services

- `IBusinessProfileQueryService`
- `ISalonCatalogQueryService`
- `IGalleryQueryService`
- `ITestimonialQueryService`
- `IFaqQueryService`

These services:

- query with `AsNoTracking()`
- project directly to response DTOs
- own paging, sorting, filtering rules

### Command services

- `IInquiryCommandService`
- later:
  - `IServiceAdminCommandService`
  - `IGalleryAdminCommandService`
  - `IBusinessProfileAdminCommandService`

These services:

- enforce business rules
- create/update aggregates
- call `SaveChangesAsync`
- log business events

### Repository guidance

Do not introduce generic repositories over EF Core. EF already is the unit of work plus repository. Add narrow repositories only when:

- a query is reused across multiple services
- the query is complex enough to hide provider-specific details

## 6. Controller / Endpoints Design

Split public read endpoints from operational/admin endpoints.

### Public endpoints

- `GET /api/business/profile`
- `GET /api/services`
- `GET /api/gallery-items`
- `GET /api/testimonials`
- `GET /api/faqs`
- `POST /api/booking-requests`
- `POST /api/contact-inquiries`

These match the current public site well.

### Recommended future admin endpoints

- `GET /api/admin/inquiries`
- `PATCH /api/admin/inquiries/{id}/status`
- `POST /api/admin/services`
- `PUT /api/admin/services/{id}`
- `DELETE /api/admin/services/{id}`
- same pattern for gallery, FAQ, testimonials, business profile

### Controller rules

- controllers stay thin
- no EF code in controllers
- no mapping logic in controllers beyond passing DTOs into services
- cancellation tokens on all async actions
- explicit response types for OpenAPI

## 7. Logging Design

Use structured logging with request correlation.

### Log categories

- request lifecycle
- business actions
- persistence failures
- startup/migration/seed events

### What to log

- successful inquiry creation with `InquiryId`, `InquiryType`, `SourcePage`
- content query diagnostics at `Debug`
- migration start/end
- seed start/end and changed record counts
- handled exceptions and validation failures at appropriate levels

### What not to log

- full inquiry message body
- raw personal data when avoidable
- secrets or connection strings

### Recommended implementation

- continue using console logging in development
- preserve trace and span IDs
- add logging scopes per request
- define event IDs for key business flows
- consider Serilog later if sinks and richer enrichment are needed

## 8. Exception Handling Design

Use centralized exception handling only.

### Exception mapping

- `DomainValidationException` -> `400`
- `NotFoundException` -> `404`
- `ConflictException` -> `409`
- `DbUpdateException` -> `409` or `500` depending on cause
- unhandled exceptions -> `500`

### Current improvement

The existing global exception handler is a solid base. Extend it so API responses always return consistent `ProblemDetails`, and MVC page redirects remain separate for non-API routes.

### Recommended `ProblemDetails` extensions

- `traceId`
- `errorCode`
- optionally `correlationId`

## 9. Test Strategy

Use a layered test strategy.

### Unit tests

Cover:

- query service filtering and sorting
- command service validation and mapping
- domain rule helpers
- seed builder logic

### Integration tests

Cover:

- API endpoints with `WebApplicationFactory`
- validation responses
- database persistence behavior
- exception-to-problem-details mapping

### Persistence tests

Use SQLite file-backed integration tests for:

- indexes and constraints that SQLite supports
- migration application
- seed idempotency

Add targeted PostgreSQL tests later, ideally via containerized test runs, for:

- provider differences
- date/time behavior
- case-sensitivity and collation-sensitive queries

### Architecture tests

Add tests that assert:

- controllers do not reference `ApplicationDbContext`
- `Api` does not reference `Infrastructure.Persistence` types directly
- DTOs are not entities

## 10. Seed Data Strategy

Split seed logic into two categories.

### Static reference/content seed

Seed only canonical startup content:

- business profile
- business hours
- starter catalog
- starter testimonials
- starter FAQ
- starter gallery

This should be:

- idempotent
- environment-aware
- safe to run repeatedly

### Non-static data

Do not seed inquiries in production. For local/demo environments, optionally add sample inquiries behind a development-only flag.

### Recommendation

Replace `EnsureCreated` fallback over time with migrations-only startup. Seed after migrations are applied.

## 11. Migration Strategy: SQLite to PostgreSQL

Design for provider portability now, even before switching.

### Rules to follow now

- avoid SQLite-specific SQL in migrations
- keep EF mappings explicit
- avoid provider-specific column types unless behind conditional config
- prefer enums stored as strings
- treat text search semantics as provider-sensitive
- keep date/time usage explicit in UTC

### Recommended migration setup

Use separate migration sets per provider:

- `Infrastructure/Persistence/Migrations/Sqlite`
- `Infrastructure/Persistence/Migrations/PostgreSql`

At runtime:

- provider selected by config
- migrations assembly or history table configured per provider

### Upgrade path

1. Freeze schema changes briefly.
2. Generate PostgreSQL migrations from the same model.
3. Provision PostgreSQL database.
4. Run schema migration.
5. Bulk-copy data from SQLite to PostgreSQL via one-time import job or export/import script.
6. Validate row counts and critical spot checks.
7. Switch connection string and provider setting.
8. Run smoke tests and keep rollback path to SQLite snapshot.

### Key portability concerns

- SQLite is permissive with types; PostgreSQL is strict.
- String comparisons and sorting may behave differently.
- `DateOnly` and `TimeOnly` behavior should be verified end to end.
- Auto-increment and identity semantics differ.
- Raw SQL, if added later, must be provider-aware.

## Implementation Order

1. Normalize folder boundaries without breaking behavior: move entity configs, separate API contracts, keep one deployable web app.
2. Refactor `ApplicationDbContext` to use `IEntityTypeConfiguration<T>` classes.
3. Add auditable base entity pattern and save interceptor for timestamps.
4. Split public query services into smaller domain-focused services.
5. Introduce application/business validators for non-trivial rules.
6. Standardize `ProblemDetails` mapping for validation, domain, and persistence exceptions.
7. Add structured event IDs and request correlation logging.
8. Move seed logic into dedicated seed runner classes and make seeding fully idempotent.
9. Add missing unit and integration tests around service behavior and error handling.
10. Add provider-specific migration folders and test both SQLite and PostgreSQL startup paths.
11. Introduce admin endpoints only after public API contracts are stable.

## Risks and Mitigations

- Risk: duplicated service logic between MVC and API paths.
  - Mitigation: centralize all inquiry creation in one application command service used by both site and API controllers.
- Risk: `EnsureCreated` bypasses migration history and complicates future upgrades.
  - Mitigation: move to migrations-only schema management as soon as possible.
- Risk: provider differences between SQLite and PostgreSQL cause runtime surprises.
  - Mitigation: keep mappings explicit and add PostgreSQL integration tests before cutover.
- Risk: content entities lack consistent audit fields and publish-state rules.
  - Mitigation: add `CreatedUtc`, `UpdatedUtc`, and `IsPublished` consistently.
- Risk: validation is partly duplicated across DTOs and services.
  - Mitigation: keep simple transport validation in DTOs, move business rules into application validators.
- Risk: PII leakage through logs.
  - Mitigation: log identifiers and metadata, not full inquiry payloads.

## Tradeoffs Made and Why

- Single deployable app instead of multiple projects per layer:
  - Chosen to keep complexity proportionate for a small business backend while still enforcing internal boundaries.
- EF Core directly in application services instead of generic repositories:
  - Chosen because EF already provides repository/unit-of-work behavior and generic repositories usually add ceremony without value.
- One `Inquiry` table for bookings and contact messages:
  - Chosen because operational workflows are similar and the domain is still small; subtype tables would be premature.
- String-backed display pricing and duration:
  - Chosen because the current business need is content publishing, not financial calculation; normalized pricing can be added later if booking/payment flows deepen.
- Separate migrations per provider:
  - Chosen to reduce cutover risk and keep provider-specific DDL predictable during the SQLite-to-PostgreSQL transition.

## Notes Specific to the Current Codebase

The existing codebase already aligns well with this target in a few important ways:

- API contracts are already separated from entities.
- Application services already exist.
- Global exception handling and `ProblemDetails` are already present.
- The app already supports both SQLite and PostgreSQL provider selection.

The biggest architectural gaps to close next are:

- remove duplicated inquiry creation logic between MVC services and API services
- move EF configurations out of `OnModelCreating`
- replace `EnsureCreated` fallback with migration-first startup
- add a clearer boundary between public query services and future admin command services
