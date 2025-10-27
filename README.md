# SolidApiExample

This project is a reference implementation of my .NET architecture approach focused on clean design, test-first development, and layered separation of concerns.

It models a simple customer and order domain using MediatR for orchestration, strict separation of concerns, and full test coverage from unit to integration.

> Want the big picture view? See the [architecture doc](docs/architecture.md).

## Table of Contents

- [Why This Exists](#why-this-exists)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Running Locally](#running-locally)
- [Configuration](#configuration)
- [Testing & Quality](#testing--quality)
- [API Documentation](#api-documentation)
- [Design & Patterns](#design--patterns)
- [SOLID Principles](#solid-principles)
- [CI / CD](#ci--cd)

---

## Why This Exists

This project is a reference implementation of my .NET architecture approach focused on clean design, test-first development, and layered separation of concerns.

It serves as a teaching and reference example for how I structure modern .NET APIs with clear boundaries, strong tests, and measurable quality.

---

## Tech Stack

- **Runtime:** .NET 9 (ASP.NET Core), MediatR
- **Testing:** xUnit, Moq, Coverlet, Stryker, WebApplicationFactory integration tests
- **Quality Gates:** SonarCloud, ReportGenerator
- **Docs & Tooling:** Swagger (Swashbuckle) with XML comments
- **CI/CD:** GitHub Actions for build, test, coverage, and artifact upload

---

## Architecture

This project demonstrates how I structure layered .NET applications for clarity, change safety, and testability.

```
src/
 ├─ Api              # HTTP entry point, authentication, DI, swagger
 ├─ Application      # Requests, handlers, DTOs, pipeline behaviors, validators
 ├─ Domain           # Order and Customer aggregates and invariants
 └─ Infrastructure   # Repository implementations (currently in-memory)
tests/
 └─ TestSuite        # Unit and integration tests
```

> For the complete system overview, request flow, and testing strategy, see [docs/architecture.md](docs/architecture.md).

---

## Running Locally

```bash
# Restore packages
dotnet restore

# Build the solution
dotnet build

# Launch the API (https://localhost:7245/ by default)
dotnet run --project src/Api/SolidApiExample.Api.csproj
```

### Configuration

- API key authentication expects header `X-Api-Key` (default: `dev-api-key`)
- Override via `appsettings.json` or environment variable `Auth:ApiKey`

---

## Testing & Quality

```bash
# Run unit and integration tests
dotnet test

# Run mutation tests
dotnet stryker
```

- Unit test coverage around 95%
- Mutation score around 85%
- Coverlet and ReportGenerator output feeds SonarCloud for visualization and gating
- Integration tests (WebApplicationFactory<Program>) verify API key authentication and full request flow through the in-memory repositories
- Swagger docs are generated from XML comments and published as CI artifacts

---

## API Documentation

Swagger is enabled with XML comments.

- Navigate to `/swagger` when running locally
- CI publishes `swagger/swagger.json` so consumers can always access the latest OpenAPI spec

---

## Design & Patterns

This example demonstrates common patterns I use in production systems.

- **Mediator:** Commands and queries via MediatR keep controllers thin and decouple orchestration from I/O
- **Repository:** ICustomersRepo and IOrdersRepo abstract persistence; Infrastructure provides in-memory adapters
- **Pipeline:** Logging and validation behaviors apply cross-cutting rules consistently
- **Domain Model:** Aggregates (Order, Customer) enforce rules internally
- **Value Objects:** Email and Money validate themselves at construction time
- **Factory Methods:** Create() and FromExisting() ensure valid domain state
- **Dependency Injection:** Interfaces wired in Program.cs; layers depend on abstractions, not implementations
- **Guard Clauses:** Defensive checks at boundaries keep methods small and explicit

---

## SOLID Principles

- **Single Responsibility:** Each handler owns one use case; value objects and controllers do only one thing well
- **Open/Closed:** Add behaviors, handlers, or repository implementations without modifying existing logic
- **Liskov Substitution:** In-memory repos substitute for EF or Dapper with no surprises
- **Interface Segregation:** Narrow repository interfaces per aggregate
- **Dependency Inversion:** Domain is dependency free; infrastructure plugs in at composition root

---

## CI / CD

GitHub Actions (.github/workflows/ci.yml) handles:

- Build and test execution with coverage reporting
- SonarCloud analysis for quality gates and metrics
- Ready to extend with dependency scanning or deployment steps
