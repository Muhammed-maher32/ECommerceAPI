# ECommerce API

A production-oriented e-commerce backend built with **.NET 10** and ASP.NET Core, organized as a Clean Architecture solution. It exposes a versioned REST API (`/api/v1`) for catalog browsing, basket management, ordering, and user authentication.

## Tech stack

| Layer | Technology |
| --- | --- |
| Web | ASP.NET Core minimal APIs, API versioning, Swagger (Dev only) |
| Application | MediatR, FluentValidation, Mapster, Ardalis.Specification |
| Data | Entity Framework Core 10 + Npgsql (PostgreSQL 17) |
| Identity | ASP.NET Identity + JWT Bearer (HS256) with refresh-token rotation |
| Caching | ASP.NET Core Output Cache + Microsoft.Extensions.Caching.Hybrid backed by Redis |
| Infrastructure | Docker Compose (PostgreSQL + Redis), Dockerfile for the API |

## Project layout

```
ECommerce.API/            Presentation layer: endpoints, middleware, DI composition
ECommerce.UseCases/       Application layer: commands/queries, validation, DTOs
ECommerce.Domain/         Domain layer: entities, constants, repository contracts
ECommerce.Infrastructure/ Persistence (EF Core), Identity, caching, seeding, read services
```

Cross-cutting rules that keep the codebase honest:

- **No N+1 queries** — all navigation loads are SQL-side projections (no lazy loading).
- **Soft deletes** — every store entity inherits `BaseEntity` and is soft-deleted through a global query filter.
- **Fail-fast configuration** — `Jwt` and basket cache options are validated at startup; a missing Production `Redis` connection string throws rather than silently degrading.
- **Secrets stay out of git** — `.env` is ignored; only `.env.example` is tracked. Real values arrive via environment variables or user secrets.

## Features

- Browse products with paging, search, filtering by brand/type, and sorting
- Anonymous baskets (via `X-Buyer-Id`) upgraded to authenticated baskets on login
- Orders priced server-side from the catalog — prices never come from the client
- JWT auth (15-minute access tokens) with rotating, hashed refresh tokens and reuse detection
- Password change, forgot-password, and reset flows
- Output-cached product search and delivery-method endpoints

## Getting started (development)

Prerequisites: .NET 10 SDK, Docker (for PostgreSQL + Redis).

1. **Start the infrastructure** — copy `.env.example` to `.env`, fill in values, then run:

   ```sh
   docker compose up -d
   ```

2. **Supply secrets** — the app reads the database connection string (`ConnectionStrings:DefaultConnection`), `Jwt:*`, and the Super Admin seed (`Seed:SuperAdmin`) from appsettings/user-secrets/environment. With the `ECommerce.API` project selected:

   ```sh
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=127.0.0.1;Port=5432;Database=ECommerceDb;Username=postgres;Password=change_me"
   dotnet user-secrets set "Jwt:Secret" "a-random-string-at-least-32-bytes-long"
   dotnet user-secrets set "Jwt:Issuer" "http://localhost:5254"
   dotnet user-secrets set "Jwt:Audience" "http://localhost:5254"
   ```

3. **Run**:

   ```sh
   dotnet run --project ECommerce.API
   ```

   In Development the app applies EF migrations, seeds the catalog (brands, types, products, delivery methods, roles) automatically, and exposes Swagger at `http://localhost:5254/swagger`.

## Configuration reference

All settings can be overridden with environment variables (double underscores `__` for nesting).

| Variable | Required | Notes |
| --- | --- | --- |
| `ConnectionStrings__DefaultConnection` | Yes | Npgsql connection string for the single shared database |
| `ConnectionStrings__Redis` | Production | Missing in Production fails startup; optional in Development (falls back to in-memory HybridCache) |
| `Jwt__Secret` | Yes | Must be at least 32 bytes |
| `Jwt__Issuer` / `Jwt__Audience` | Yes | Validated against the token at login |
| `Jwt__AccessTokenExpirationMinutes` | No | Default 15 |
| `Jwt__RefreshTokenExpirationDays` | No | Default 7 |
| `Database__MigrateOnStartup` | No | `true` applies migrations + idempotent seeding on boot (Production bootstrap; leave off afterwards) |
| `Seed__SuperAdmin__Email/Password/DisplayName` | No | Creates/resyncs the Super Admin account during seeding |

## Production deployment

The API ships as a Docker image (`ECommerce.API/Dockerfile`, multi-stage, non-root default user) that exposes port **8080**:

```sh
docker build -f ECommerce.API/Dockerfile -t ecommerce-api .
```

Run it with PostgreSQL and Redis, passing the required configuration:

```sh
docker run -d -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ConnectionStrings__DefaultConnection="Host=postgres;Port=5432;Database=ECommerceDb;Username=postgres;Password=change_me" \
  -e ConnectionStrings__Redis="redis:6379" \
  -e Jwt__Secret="a-random-string-at-least-32-bytes-long" \
  -e Jwt__Issuer="https://yourdomain.com" \
  -e Jwt__Audience="https://yourdomain.com" \
  -e Database__MigrateOnStartup="true" \
  ecommerce-api
```

Notes:

- Set `Database__MigrateOnStartup=true` for the **first** deploy so the schema, seed data, and roles are created. Turn it off afterwards — migrations are non-destructive, but the Super Admin password is re-synchronised from configuration on every seed run.
- Swagger is **not** exposed in Production.
- Baskets live in Redis only; do not run Production without Redis. Put a reverse proxy (e.g. Caddy/Traefik/nginx) in front to terminate TLS.

## API overview

All routes live under `/api/v{version}` (currently `1.0`) and respond in a consistent `ApiResponse<T>` envelope:

- `POST /api/v1/users/register, /login, /refresh, /logout, /forgot-password, /reset-password, /change-password`
- `GET  /api/v1/users/me`
- `GET  /api/v1/products`, `GET /api/v1/products/{id}`, `GET /api/v1/products/paged`
- `GET  /api/v1/brands`, `GET /api/v1/types`, `GET /api/v1/delivery-methods`
- `GET/POST/DELETE /api/v1/baskets`
- `GET/POST /api/v1/orders`

Roles (`SuperAdmin`, `Admin`, `User`) exist in the seed data; endpoints currently only require an authenticated user.

## Roadmap

Planned work to close remaining production gaps:

- **Email service** — SMTP provider integration to send confirmation, password-reset, and transactional emails (the identity token providers are already in place).
- **Rate limiting** — throttle authentication endpoints to blunt brute-force attempts against `/login`, `/register`, and `/refresh`.
- **Health checks** — expose `/health/live` and `/health/ready` (database + Redis) and add a Docker `HEALTHCHECK` to the API image.
- **Grafana** — ship metrics (OpenTelemetry) with a Grafana dashboard for requests, latency, and infrastructure monitoring.