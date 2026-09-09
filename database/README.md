# Database setup

The API is configured for SQL Server LocalDB by default.

1. Open the solution in Visual Studio.
2. Make sure `LogiFlow.API` is the startup project.
3. Open Package Manager Console.
4. Run:

```powershell
Add-Migration InitialCreate -Project LogiFlow.Infrastructure -StartupProject LogiFlow.API
Update-Database -Project LogiFlow.Infrastructure -StartupProject LogiFlow.API
```

The API also calls `Database.Migrate()` on startup, so after the first migration exists it will apply pending migrations automatically.

Default development admin:
- Email: `admin@logiflow.local`
- Password: `Admin@12345`

Change these values before using the project outside local development.
