# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Personal blog system (runs <https://nikolay.it> and a couple of other blogs) built with ASP.NET Core MVC on .NET 10, EF Core 10 + SQL Server. The solution is `src/BlogSystem.sln`; there is no code outside `src/`. The README still says ASP.NET Core 3.1 — it is outdated.

## Commands

Run from `src/`:

```powershell
dotnet build BlogSystem.sln
dotnet test BlogSystem.sln
dotnet test Tests/BlogSystem.Services.Data.Tests --filter "FullyQualifiedName~<TestName>"   # single test
dotnet run --project Web/BlogSystem.Web
```

- Running the web app requires a local SQL Server (default connection: `Server=.;Database=nikolay.it;Trusted_Connection=True`). Migrations are applied and seeders run automatically on every startup (`Startup.Configure`), so no manual `database update` is needed.
- Tests use xunit.v3 + Moq + EF InMemory in `Tests/BlogSystem.Services.Data.Tests`. xunit.v3 test projects are executables (`<OutputType>Exe</OutputType>`); note that a xunit.v3 project with zero tests makes `dotnet test` fail with "Could not launch test process", so keep at least one test in the project.
- `Tests/Sandbox` is a console playground already wired with the full DI container, DbContext, and seeding — put throwaway experiment code in its `SandboxCode` method.

### EF Core migrations

Migrations live in `src/Data/BlogSystem.Data/Migrations`. The `DesignTimeDbContextFactory` reads `appsettings.json` from the current directory, so run from the web project:

```powershell
cd src/Web/BlogSystem.Web
dotnet ef migrations add <Name> --project ..\..\Data\BlogSystem.Data --startup-project .
```

## Architecture

Layered solution; dependencies flow Web → Services → Data → Common:

- `BlogSystem.Common` — `GlobalConstants` (system name, base URL, `AdministratorRoleName`).
- `Data/BlogSystem.Data.Models` — entities: `BlogPost`, `Page`, `Video`, `Setting`, plus Identity's `ApplicationUser`/`ApplicationRole`. Entities inherit `BaseModel<TKey>` / `BaseDeletableModel<TKey>`.
- `Data/BlogSystem.Data.Common` — base model and repository abstractions.
- `Data/BlogSystem.Data` — `ApplicationDbContext` (an `IdentityDbContext`), EF repository implementations, migrations, seeders (`RolesSeeder`, `SettingsSeeder`).
- `Services/*` — `Services` (YouTube video fetcher, `BlogUrlGenerator`), `Services.Data` (DB-backed services), `Services.Mapping` (Mapster conventions).
- `Web/BlogSystem.Web.ViewModels` — all view models; `Web/BlogSystem.Web` — MVC app with default Identity UI.

### Data access conventions

- Controllers and services never take `ApplicationDbContext` directly — they inject `IRepository<T>` / `IDeletableEntityRepository<T>` (registered as open generics in `Startup`).
- Soft delete is the default: `Delete()` only sets `IsDeleted`; a global query filter hides deleted rows from `All()`. Use `AllWithDeleted()`, `GetByIdWithDeletedAsync()`, `HardDelete()`, `Undelete()` to bypass it.
- `ApplicationDbContext.SaveChanges*` automatically stamps `CreatedOn`/`ModifiedOn` on `IAuditInfo` entities; all cascade deletes are converted to `Restrict` in `OnModelCreating`.
- The old migrations predate EF Core 10, so EF's pending-model-changes check is suppressed (`ConfigureWarnings(... Ignore(RelationalEventId.PendingModelChangesWarning))` in `Startup` and Sandbox, same approach as dela.bg). When you change entities, still add a migration as usual.

### View model mapping (Mapster by convention)

- A view model opts into mapping by implementing `IMapFrom<TEntity>`, `IMapTo<T>`, or `IHaveCustomMappings` (`CreateMappings(TypeAdapterConfig configuration)` using `configuration.NewConfig<TSource, TDest>().Map(...)`) — there are no manual profile classes.
- Mappings are registered reflectively at startup (`MappingConfig.RegisterMappings`) from the `BlogSystem.Web.ViewModels` assembly only, so mappable view models must live in that project.
- Query pattern: project inside the DB query with the `.To<TViewModel>()` IQueryable extension (`BlogSystem.Services.Mapping`), e.g. `this.blogPosts.All().Where(...).To<BlogPostViewModel>().FirstOrDefault()`.
- Gotcha: Mapster ships its own `Mapster.IMapFrom<>`, so in a file that has `using Mapster;` implement the project's interface fully qualified: `BlogSystem.Services.Mapping.IMapFrom<T>`.

### Web conventions

- Public controllers extend `BaseController`. Admin CRUD controllers live in the `Administration` area and extend `AdministrationController`, which applies `[Authorize(Roles = Administrator)]` and `[Area("Administration")]`.
- Custom routes are defined in `Startup.Configure`: blog posts use `Blog/{year}/{month}/{title}/{id}`, static pages use `Pages/{permalink}`.
- Client-side assets: `wwwroot/` with LibMan (`libman.json`) restoring libraries at build time. CSS/JS bundling and minification happen at runtime via LigerShark.WebOptimizer: `Startup.ConfigureServices` defines the `/css/site.min.css` and `/js/site.min.js` bundles (there are no physical min files), `app.UseWebOptimizer()` runs before `UseStaticFiles()`, and `@addTagHelper *, WebOptimizer.Core` in `_ViewImports.cshtml` provides cache busting.

### Configuration and disabled features

- Secrets (YouTube API key, production connection string) belong in `appsettings.Production.json`, which is intentionally not tracked in git.
- Hangfire is intentionally disabled — the packages are still referenced and its setup is commented out in `Startup` (recent commits removed feed processing to another system). Do not re-enable or "clean up" the commented Hangfire code without being asked.

## Code style

StyleCop.Analyzers runs on every project with shared config (`src/Rules.ruleset`, `src/stylecop.json`). The non-default conventions to follow:

- `using` directives go **inside** the namespace, `System.*` first, blank line between using groups.
- Instance members are accessed with the `this.` prefix.
- Files end with a newline.
