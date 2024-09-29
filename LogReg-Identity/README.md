# LogReg-Identity

This project is an ASP.NET Core 8.0 application demonstrating Identity with role-based permissions, notes, menus and a repository/service pattern.

Quick start

1. Ensure PostgreSQL is running and update the connection string in `appsettings.json`.
2. From project folder run:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

What I changed (2026-02-17)

- Introduced `IUserRepository` / `UserRepository` and `IUserService` / `UserService`.
- Refactored `HomeController` to use the service layer.
- Fixed migration SQL Server type usages and seed ordering for roles.
- Modernized the site layout and CSS to use a Bootstrap 5 CDN and a cleaner navbar.

Next recommended improvements

- Refactor other controllers to use services.
- Add unit/integration tests for services and repositories.
- Add CI pipeline and Dockerfile improvements.

License: MIT
