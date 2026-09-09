# LogiFlow - Member 1 Backend

Complete backend foundation for Member 1 of the LogiFlow project specification.

## Owned modules

- M1 Authentication & Authorization
- M2 Customer/User Management
- M3 Vehicle & Driver Management

## Architecture

```text
LogiFlow.API            -> Controllers, middleware, JWT configuration
LogiFlow.Application    -> DTOs, interfaces, services, validators
LogiFlow.Domain         -> Entities, enums, constants
LogiFlow.Infrastructure -> EF Core, SQL Server, security, seeding
```

## Technology

- ASP.NET Core Web API
- .NET 8
- Entity Framework Core 8
- SQL Server / LocalDB
- JWT Bearer authentication
- ASP.NET Core Identity PasswordHasher
- FluentValidation
- Swagger

## Run

1. Open `LogiFlow.sln` in Visual Studio 2022/2026.
2. Restore NuGet packages.
3. Set `LogiFlow.API` as startup project.
4. Run the migration commands in `database/README.md`.
5. Start the API and open Swagger.

## Important

The JWT key and seeded admin credentials in `appsettings.json` are development values. Replace them with environment-specific secrets before production use.

## Member 1 API routes

### Authentication
- POST `/api/auth/register`
- POST `/api/auth/login`
- POST `/api/auth/refresh-token`
- POST `/api/auth/logout`
- POST `/api/auth/forgot-password`
- POST `/api/auth/reset-password`
- POST `/api/auth/change-password`
- GET `/api/auth/me`

### Customers
- GET `/api/customers/profile`
- PUT `/api/customers/profile`
- GET `/api/customers/addresses`
- POST `/api/customers/addresses`
- PUT `/api/customers/addresses/{id}`
- DELETE `/api/customers/addresses/{id}`
- GET `/api/customers/{id}`

### Vehicles
- GET/POST `/api/vehicles`
- GET/PUT/DELETE `/api/vehicles/{id}`
- PATCH `/api/vehicles/{id}/status`

### Drivers
- GET/POST `/api/drivers`
- GET/PUT/DELETE `/api/drivers/{id}`
- PATCH `/api/drivers/{id}/availability`
