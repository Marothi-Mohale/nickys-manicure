# Nicky’s Manicure & Pedicure

Production-minded ASP.NET Core web application for a luxury beauty salon, combining a premium client-facing experience with a maintainable backend foundation.

## Table of Contents

- [Overview](#overview)
- [Feature Highlights](#feature-highlights)
- [Architecture Overview](#architecture-overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Environment Configuration](#environment-configuration)
- [Database Setup](#database-setup)
- [Run Locally](#run-locally)
- [Backend Capabilities](#backend-capabilities)
- [Validation, Error Handling, and Fault Tolerance](#validation-error-handling-and-fault-tolerance)
- [API Documentation](#api-documentation)
- [Health Checks](#health-checks)
- [Seed Data](#seed-data)
- [Logging and Observability](#logging-and-observability)
- [Testing](#testing)
- [Scalability and Future Improvements](#scalability-and-future-improvements)
- [Screenshots](#screenshots)
- [Deployment Readiness](#deployment-readiness)
- [License and Ownership](#license-and-ownership)
- [Author](#author)

## Overview

Nicky’s Manicure & Pedicure is a commercial-style salon web application designed for a real service business rather than as a demo-only UI. The product supports a premium public website, structured service discovery, booking requests, contact enquiries, gallery and testimonial presentation, and an API surface prepared for future admin workflows.

From an engineering perspective, the project is built to demonstrate disciplined backend design in a small-business context:

- clear separation between public API concerns, application services, domain entities, and infrastructure
- DTO-based contracts rather than direct entity exposure
- validation integrated into the request pipeline
- structured logging, health checks, and consistent error responses
- SQLite for low-friction deployment today, with deliberate preparation for future migration to PostgreSQL or SQL Server

This repository is intended to present delivery quality that is commercially credible, maintainable, and suitable for further product growth.

## Feature Highlights

- Premium salon website experience with dedicated Home, About, Services, Gallery, Booking, and Contact flows
- Public service catalog with filtering, sorting, category support, and featured services
- Booking request workflow with validation, persistence, admin-ready read APIs, and status transitions
- Contact enquiry workflow with validation, persistence, admin-ready read APIs, and status transitions
- Business profile and business hours endpoints for frontend consumption
- Testimonials, FAQ, and gallery metadata endpoints for public website content
- Swagger/OpenAPI support for backend exploration
- Health check endpoints for liveness/readiness scenarios
- Structured startup seed data for local/demo environments
- Global exception handling with `ProblemDetails` responses
- FluentValidation-based request validation
- Automated tests covering service logic, validation, API behavior, paging/filtering, and error flows
- Real salon imagery and logo integrated into the frontend with a maintainable asset structure

## Architecture Overview

The solution is intentionally implemented as a modular monolith.

This is a good fit for the product stage: one deployable application, but with clean internal boundaries that make future extraction or expansion straightforward.

### Design principles

- Keep controllers thin
- Keep business logic in application services
- Keep EF Core usage behind `ApplicationDbContext` without adding repository ceremony
- Use DTOs for external contracts
- Use FluentValidation for write request validation
- Centralize error handling and logging in middleware
- Keep provider-specific database details isolated as much as practical

### High-level flow

1. HTTP requests enter ASP.NET Core controllers.
2. FluentValidation validates incoming write/query models.
3. Controllers delegate to focused application services.
4. Services use `ApplicationDbContext` and EF Core configurations for persistence.
5. Exceptions are normalized into `ProblemDetails` responses by middleware.
6. Logging, health checks, and startup seeding support operational visibility.

## Tech Stack

### Backend

- **ASP.NET Core Web API / MVC on .NET 8**
  - Mature platform for API and server-rendered web delivery
  - Strong middleware, DI, validation, and observability support

- **C#**
  - Expressive, type-safe language well suited to layered backend design

- **Entity Framework Core**
  - Productive ORM for domain modeling, validation support, relational mapping, and future provider migration

- **SQLite**
  - Practical default for low-friction local and small-scale deployment scenarios
  - Suitable for current scope while keeping migration paths open

### Frontend

- **ASP.NET Core Razor Views**
  - Appropriate for a lean, server-rendered marketing and booking site
  - Fast to deliver, simple to host, easy to maintain

- **Bootstrap + custom CSS**
  - Used as a base, with a branded premium visual layer on top

### Quality and Operations

- **FluentValidation**
  - Clean request validation rules with consistent pipeline integration

- **Swagger / OpenAPI**
  - API discoverability for frontend work, QA, and integration review

- **xUnit**
  - Practical automated testing strategy across validators, services, and API behavior

## Project Structure

```text
NickysManicurePedicure/
├── Api/
│   └── Controllers/
│       ├── Admin/
│       └── ...
├── Application/
│   ├── Abstractions/
│   └── Services/
├── Common/
│   └── Exceptions/
├── Content/
├── Data/
│   ├── Configurations/
│   ├── ApplicationDbContext.cs
│   ├── DatabaseProviderConfigurator.cs
│   └── SeedDataExtensions.cs
├── Dtos/
│   ├── Common/
│   ├── Requests/
│   └── Responses/
├── Extensions/
├── Infrastructure/
├── Middleware/
├── Models/
│   ├── Entities/
│   └── Options/
├── Validation/
├── ViewModels/
├── Views/
├── wwwroot/
│   ├── css/
│   ├── images/
│   └── js/
├── Program.cs
└── appsettings.json

NickysManicurePedicure.Tests/
├── Api/
├── Infrastructure/
├── Services/
├── Validation/
└── ...
```

### Structure notes

- `Application/Services` contains business use cases and orchestration logic
- `Data/Configurations` contains EF Core Fluent API mappings
- `Dtos` defines stable API contracts
- `Middleware` centralizes exception handling, status-code normalization, and request logging
- `Validation` contains FluentValidation rules
- `Views` and `wwwroot` support the premium server-rendered frontend
- `Tests` are split by responsibility rather than by framework artifact

## Getting Started

### Prerequisites

- .NET SDK 8.0
- Git

### Clone the repository

```bash
git clone <repository-url>
cd nickys-manicure
```

### Restore dependencies

```bash
dotnet restore NickysManicurePedicure/NickysManicurePedicure.csproj
dotnet restore NickysManicurePedicure.Tests/NickysManicurePedicure.Tests.csproj
```

## Environment Configuration

Application configuration is stored in:

- `NickysManicurePedicure/appsettings.json`
- `NickysManicurePedicure/appsettings.Development.json`

Key configuration areas include:

- database provider and startup behavior
- SQLite connection string
- business profile details
- logging levels

Example configuration shape:

```json
{
  "Database": {
    "Provider": "Sqlite",
    "ApplyMigrationsOnStartup": false,
    "SeedOnStartup": true
  },
  "ConnectionStrings": {
    "SqliteConnection": "Data Source=App_Data/nickys-manicure-backend.db"
  },
  "BusinessProfile": {
    "Name": "Nicky's Manicure & Pedicure",
    "Phone": "068 251 8739",
    "Email": "nickyshunje@gmail.com"
  }
}
```

## Database Setup

The current persistence provider is SQLite via EF Core.

### Current behavior

- The app is configured for SQLite by default
- On startup, the application ensures the schema exists
- If enabled, seed data is applied for local/demo use

### Important note

This project is intentionally structured for a future move to formal migration-first schema management. At present:

- SQLite is fully supported for local and current application use
- startup falls back to `EnsureCreated()` when no migrations are present
- adding the initial EF Core migration is a recommended next production step

### SQLite database file

By default, the application stores the database at:

```text
NickysManicurePedicure/App_Data/nickys-manicure-backend.db
```

## Run Locally

### Standard run

```bash
dotnet run --project NickysManicurePedicure/NickysManicurePedicure.csproj
```

### Run with an explicit local URL

```bash
ASPNETCORE_ENVIRONMENT=Development \
ASPNETCORE_URLS=http://127.0.0.1:5099 \
dotnet run --project NickysManicurePedicure/NickysManicurePedicure.csproj
```

### Build explicitly

```bash
dotnet build NickysManicurePedicure/NickysManicurePedicure.csproj
dotnet build NickysManicurePedicure.Tests/NickysManicurePedicure.Tests.csproj
```

## Backend Capabilities

### Public website and content

- business profile and business hours
- published services and service categories
- published testimonials
- published FAQ items
- gallery metadata and curated visuals

### Booking workflow

- create booking requests
- retrieve bookings for admin-ready scenarios
- get booking details by ID
- update booking status for future back-office workflows

### Contact enquiry workflow

- create enquiries
- retrieve enquiries for admin-ready scenarios
- get enquiry details by ID
- update enquiry status for future back-office workflows

### Query capabilities

- filtering by category and featured state where relevant
- sorting by display order and supported fields
- pagination metadata on list endpoints
- validation for page size, sorting, and query rules

## Validation, Error Handling, and Fault Tolerance

### Validation

The application validates write requests with FluentValidation, including:

- required fields
- max lengths
- email format
- phone number sanity
- booking date/time rules
- page size and query parameter guards
- admin-ready upsert request validation for future content management

Validation failures return structured `ValidationProblemDetails` responses for API consumers.

### Error handling

Global middleware provides consistent error behavior for:

- validation failures
- bad requests
- not found conditions
- conflict-ready cases
- unhandled server exceptions

The API returns `ProblemDetails`-style JSON and avoids leaking sensitive implementation detail in production-style responses.

### Fault tolerance

- centralized exception handling
- correlation-friendly logging
- health check endpoints
- idempotent startup seeding
- startup database initialization with explicit warnings about migration maturity

## API Documentation

Swagger/OpenAPI is enabled in development.

Once the app is running, the Swagger UI is available at:

```text
/swagger
```

The raw OpenAPI document is available at:

```text
/swagger/v1/swagger.json
```

This is useful for:

- frontend integration work
- API review
- QA verification
- endpoint contract inspection

## Health Checks

The backend exposes health endpoints suitable for operational checks:

- `/health`
- `/api/health`
- `/health/ready`
- `/health/live`

The health responses include structured JSON and are designed to support future observability tooling.

## Seed Data

The project includes practical local/demo seed data for:

- service categories
- manicure and pedicure services
- business profile
- business hours
- testimonials
- FAQs
- gallery items

Seed behavior is designed to be repeatable and maintainable for local development and demos.

## Logging and Observability

The application includes practical observability basics:

- structured request logging
- centralized unhandled exception logging
- warning-level logging for suspicious or invalid states
- correlation ID support
- health endpoints for readiness/liveness checks

The implementation is intentionally lightweight, appropriate for a small-business product, while leaving room for future integration with centralized logging, metrics, or tracing platforms.

## Testing

The solution includes a focused automated test suite built with xUnit.

### Coverage areas

- service-layer unit tests
- request validation tests
- integration tests for core API endpoints
- error-handling behavior tests
- pagination/filtering/sorting tests

### Run tests

```bash
dotnet test NickysManicurePedicure.Tests/NickysManicurePedicure.Tests.csproj
```

### Testing philosophy

The suite prioritizes high-value coverage over inflated numbers. The goal is confidence in:

- core business workflows
- contract correctness
- validation behavior
- operational safety

## Scalability and Future Improvements

This codebase is intentionally prepared for further growth.

### Near-term improvements

- add formal EF Core migrations and migration history
- add authentication and authorization for admin routes
- add back-office CRUD for services, testimonials, FAQs, gallery, and business profile
- add email/SMS/WhatsApp notification implementations behind existing seams
- add media upload handling for gallery and branding assets

### Medium-term improvements

- migrate from SQLite to PostgreSQL or SQL Server when operational needs grow
- add CI/CD pipeline and deployment automation
- add caching for public content endpoints where useful
- add richer admin reporting and booking workflow tooling

### Architectural readiness

The project already supports these future steps through:

- layered service design
- DTO separation
- centralized validation
- provider-conscious EF configuration
- isolated middleware and infrastructure concerns

## Screenshots

Screenshots can be added here as the product evolves.

Suggested additions:

- Home page hero
- Services page
- Gallery page
- Booking flow
- Swagger UI
- Health check response example

Example placeholder:

```md
![Home Page](docs/screenshots/home-page.png)
![Services Page](docs/screenshots/services-page.png)
![Swagger UI](docs/screenshots/swagger-ui.png)
```

## Deployment Readiness

The project is structured to be deployable as a real small-business web application, with several production-minded qualities already in place:

- clear configuration model
- health checks
- structured logging
- consistent API error contracts
- startup seed support
- maintainable project organization
- separation between public and future admin concerns

### Current production considerations

Before a formal production rollout, the following should be completed:

- introduce EF Core migrations
- pin and validate deployment configuration by environment
- configure production hosting and HTTPS termination
- add authentication/authorization for admin routes
- define backup and operational processes for the database provider in use

## License and Ownership

No open-source license is declared in this repository at present.

Unless otherwise stated, the contents of this repository should be treated as proprietary project work intended for portfolio, review, or client delivery purposes.

## Author

Developed as a production-minded ASP.NET Core salon platform showcasing:

- backend architecture and service-layer discipline
- API design and validation strategy
- operational hardening and fault tolerance
- premium server-rendered frontend execution
- commercial awareness in product delivery

For technical review, portfolio discussion, or implementation detail walkthroughs, the repository is intended to stand on its own as a credible senior-level engineering sample.
