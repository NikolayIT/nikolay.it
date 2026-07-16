# BlogSystem

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/NikolayIT/nikolay.it/actions/workflows/build.yml/badge.svg)](https://github.com/NikolayIT/nikolay.it/actions/workflows/build.yml)

An open source blog platform built with **ASP.NET Core MVC on .NET 10**, **Entity Framework Core 10**, and **SQL Server**. It powers <https://nikolay.it>.

## Features

- 📝 **Blog posts** with SEO-friendly URLs (`/Blog/{year}/{month}/{title}/{id}`)
- 📄 **Static pages** served by permalink (`/Pages/{permalink}`)
- 🎬 **Videos section** with automatic metadata fetching from the YouTube API
- ⚙️ **Database-backed settings** with seeding on startup
- 🔐 **Administration area** (ASP.NET Core Identity) with CRUD for posts, pages, videos, and settings
- 🗑️ **Soft delete** by default for all deletable entities, with global query filters
- 🎨 **Bootstrap 5** and **Font Awesome 6** front end; client-side libraries restored via LibMan at build time
- 📦 Runtime CSS/JS **bundling and minification** via [LigerShark.WebOptimizer](https://github.com/ligershark/WebOptimizer)
- ✉️ Email sending through **SendGrid**
- 🚀 Migrations and seeders run **automatically on startup** — no manual database setup

## Technology stack

| Layer | Technology |
| --- | --- |
| Web | ASP.NET Core MVC (.NET 10), Razor views, ASP.NET Core Identity |
| Data access | Entity Framework Core 10, SQL Server, repository pattern |
| Mapping | [Mapster](https://github.com/MapsterMapper/Mapster) (convention-based, registered by reflection) |
| Front end | Bootstrap 5, Font Awesome 6, LibMan, WebOptimizer |
| Email | SendGrid |
| Testing | xUnit v3, Moq, EF Core InMemory |
| Code quality | StyleCop.Analyzers with shared ruleset |

## Solution structure

Everything lives in `src/`; dependencies flow **Web → Services → Data → Common**.

```text
src/
├── BlogSystem.Common                  # Global constants (system name, base URL, role names)
├── Data/
│   ├── BlogSystem.Data.Models         # Entities: BlogPost, Page, Video, Setting, Identity models
│   ├── BlogSystem.Data.Common         # Base models and repository abstractions
│   └── BlogSystem.Data                # DbContext, repositories, migrations, seeders
├── Services/
│   ├── BlogSystem.Services            # YouTube video fetcher, blog URL generator
│   ├── BlogSystem.Services.Data       # Database-backed services
│   ├── BlogSystem.Services.Mapping    # Mapster conventions and .To<T>() projection
│   └── BlogSystem.Services.Messaging  # SendGrid email sender
├── Web/
│   ├── BlogSystem.Web.ViewModels      # All view models (mapping is registered from this assembly)
│   ├── BlogSystem.Web.Infrastructure  # Web-layer helpers
│   └── BlogSystem.Web                 # The MVC application
└── Tests/
    ├── BlogSystem.Services.Data.Tests # xUnit v3 + Moq + EF InMemory tests
    └── Sandbox                        # Console playground wired with the full DI container
```

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or full edition)

### Run the application

```powershell
git clone https://github.com/NikolayIT/nikolay.it.git
cd nikolay.it/src
dotnet run --project Web/BlogSystem.Web
```

The default connection string is `Server=.;Database=nikolay.it;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True` (see `appsettings.json`). On first run the application creates the database, applies all EF Core migrations, and seeds the `Administrator` role and default settings automatically.

### Create an administrator

No admin user is seeded. Register an account through the site's Identity UI, then assign it the `Administrator` role, e.g.:

```sql
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT u.Id, r.Id
FROM AspNetUsers u, AspNetRoles r
WHERE u.UserName = 'you@example.com' AND r.Name = 'Administrator';
```

The administration area is then available at `/Administration`.

### Build and test

```powershell
cd src
dotnet build BlogSystem.sln
dotnet test BlogSystem.sln
```

To run a single test:

```powershell
dotnet test Tests/BlogSystem.Services.Data.Tests --filter "FullyQualifiedName~<TestName>"
```

`Tests/Sandbox` is a console playground pre-wired with the full DI container, DbContext, and seeding — drop throwaway experiment code into its `SandboxCode` method.

## Configuration

Secrets are kept out of source control. Create `src/Web/BlogSystem.Web/appsettings.Production.json` (intentionally not tracked in git) with your production values:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<production connection string>"
  },
  "YouTube": {
    "ApiKey": "<YouTube Data API key>"
  },
  "SendGrid": {
    "ApiKey": "<SendGrid API key>"
  }
}
```

## Architecture notes

- **Repository pattern** — controllers and services never touch `ApplicationDbContext` directly; they inject `IRepository<T>` / `IDeletableEntityRepository<T>`, registered as open generics.
- **Soft delete** — `Delete()` only sets an `IsDeleted` flag, and a global query filter hides deleted rows. `AllWithDeleted()`, `HardDelete()`, and `Undelete()` bypass it when needed.
- **Audit info** — `SaveChanges` automatically stamps `CreatedOn`/`ModifiedOn` on entities implementing `IAuditInfo`.
- **Convention-based mapping** — a view model opts into mapping by implementing `IMapFrom<TEntity>`, `IMapTo<T>`, or `IHaveCustomMappings`; queries project directly in the database via the `.To<TViewModel>()` extension.
- **Bundling** — `/css/site.min.css` and `/js/site.min.js` are virtual bundles produced at runtime by WebOptimizer; there are no physical minified files.

### Adding an EF Core migration

The design-time factory reads `appsettings.json` from the current directory, so run from the web project:

```powershell
cd src/Web/BlogSystem.Web
dotnet ef migrations add <Name> --project ..\..\Data\BlogSystem.Data --startup-project .
```

Migrations are applied automatically the next time the application starts.

## Code style

StyleCop.Analyzers runs on every project with a shared configuration (`src/Rules.ruleset`, `src/stylecop.json`). Notable conventions:

- `using` directives go **inside** the namespace — `System.*` first, with a blank line between groups.
- Instance members are accessed with the `this.` prefix.
- Files end with a newline.

## Contributing

Contributions are welcome! Feel free to open an issue for bugs or feature ideas, or submit a pull request. Please make sure `dotnet build` and `dotnet test` pass and that the StyleCop rules are satisfied before submitting.

## License

This project is licensed under the [MIT License](LICENSE).

Copyright © 2014–2026 [Nikolay Kostov](https://nikolay.it)
