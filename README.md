# LogReg-Identity

LogReg-Identity is a minimal ASP.NET Core 8.0 reference application demonstrating how to build an Identity-enabled web app with role-based permissions, simple note management, menus and a clear repository/service separation.

**Key goals**

- Show a pragmatic structure for Identity, roles and permissions
- Demonstrate repository and service layers for controller decoupling
- Include database migrations and seed data for roles/permissions
- Keep the UI simple while using modern Bootstrap styles

**Features**

- User authentication (ASP.NET Core Identity)
- Role-based permissions and role/permission seeding
- Note CRUD operations and a small menu system
- Repository + Service pattern for business/data logic
- Exception handling and permission middleware

**Prerequisites**

- .NET 8 SDK
- PostgreSQL (local or container)

**Quick start (local, without Docker)**

1. Open `appsettings.json` and update the `ConnectionStrings:DefaultConnection` to point to your PostgreSQL instance.
2. Restore packages and apply migrations:

```bash
dotnet restore
dotnet ef database update
dotnet run --project LogReg-Identity/LogReg-Identity.csproj
```

**Using Docker Compose (recommended for PostgreSQL)**

1. Start the DB service from the repository root:

```bash
docker compose up -d postgres
```

2. Update `appsettings.json` to use the container DB connection (or use `localhost:5432` with the credentials in `docker-compose.yml`).
3. Run migrations and start the app (as above).

**Database / Migrations**

- Migrations are in the `LogReg-Identity/Migrations` folder. Use `dotnet ef database update` to apply them.
- The project seeds roles and permissions during migration; ensure the DB connection is correct before running the app.

**Running tests**
From the solution root:

```bash
dotnet test
```

**Project layout (high level)**

- `LogReg-Identity/Controllers` — MVC controllers (some refactored to use services)
- `LogReg-Identity/Data` — `ApplicationDbContext` and EF Core setup
- `LogReg-Identity/Models` — domain and DTO models
- `LogReg-Identity/Repository` — repository interfaces and implementations
- `LogReg-Identity/Services` — service layer used by controllers
- `LogReg-Identity/Middlewares` — global exception handling and permission enforcement

**Notes & next steps**

- Consider moving connection settings to environment variables for production and Docker deployments.
- Add integration tests that run against a disposable PostgreSQL instance (Testcontainers or Docker compose).
- Expand unit test coverage for `Repository` and `Service` layers.

**Contributing**
Contributions are welcome. Open an issue or a PR with a clear description and tests where relevant.

**License**
MIT

---

Updated: 2026-02-18
