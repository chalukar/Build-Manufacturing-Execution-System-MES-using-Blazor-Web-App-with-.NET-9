# MES.Solution — Manufacturing Execution System

A full-stack Manufacturing Execution System built with **.NET 9**, **Blazor WebAssembly**, **ASP.NET Core Web API**, **Entity Framework Core**, and **SQL Server** following **Clean Architecture**, **SOLID principles**, **CQRS with MediatR**, and **Domain-Driven Design**.
---
## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Technology Stack](#technology-stack)
- [Solution Structure](#solution-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Project Descriptions](#project-descriptions)
- [Known Issues and Fixes](#known-issues-and-fixes)
- [Testing](#testing)
- [Contributing](#contributing)

---

## Architecture Overview

```
┌─────────────────────────────────────────────────────┐
│           Presentation Layer                        │
│     Blazor WebAssembly (MES.Web)                    │
│  Work Orders · Production · Quality · Dashboard     │
└──────────────────┬──────────────────────────────────┘
                   │ REST API + SignalR
┌──────────────────▼──────────────────────────────────┐
│              API Layer (MES.API)                    │
│   .NET 9 Web API · JWT Auth · Swagger · SignalR     │
│   CQRS via MediatR · FluentValidation · Middleware  │
└──────────────────┬──────────────────────────────────┘
                   │ Interfaces
┌──────────────────▼──────────────────────────────────┐
│         Application Layer (MES.Application)         │
│   Commands · Queries · DTOs · Validators · Mappers  │
└──────────────────┬──────────────────────────────────┘
                   │ Domain Interfaces
┌──────────────────▼──────────────────────────────────┐
│           Domain Layer (MES.Domain)                 │
│   Entities · Enums · Exceptions · Interfaces        │
│   Pure C# — Zero external dependencies              │
└──────────────────┬──────────────────────────────────┘
                   │ Implementations
┌──────────────────▼──────────────────────────────────┐
│       Infrastructure Layer (MES.Infrastructure)     │
│   EF Core 9 · SQL Server · Repositories · UoW      │
│   Configurations · Migrations · Seeding             │
└─────────────────────────────────────────────────────┘
```

---

## Technology Stack

| Layer | Technology |
|---|---|
| Frontend | Blazor WebAssembly Standalone (.NET 9) |
| Backend API | ASP.NET Core Web API (.NET 9) |
| Database | SQL Server (LocalDB / Express) |
| ORM | Entity Framework Core 9 |
| Messaging | MediatR (CQRS pattern) |
| Validation | FluentValidation + DataAnnotations |
| Object Mapping | AutoMapper |
| Real-time | SignalR |
| Authentication | JWT Bearer Tokens |
| API Docs | Swagger / OpenAPI (Swashbuckle) |
| Unit Testing | xUnit + Moq + FluentAssertions |
| Secrets | ASP.NET Core User Secrets (dev) |

---

## Solution Structure

```
MES.Solution/                              
├── MES.Solution.sln
├── .github/
│   └── workflows/
│       └── ci.yml
│
├── src/
│   │
│   ├── MES.API/
│   │   ├── Connected Services
│   │   ├── Dependencies
│   │   ├── Properties/
│   │   ├── Controllers/
│   │   │   ├── WorkCentresController.cs
│   │   │   └── WorkOrdersController.cs
│   │   ├── Hubs/
│   │   │   └── ProductionHub.cs
│   │   ├── Middleware/
│   │   │   └── ExceptionHandlingMiddleware.cs
│   │   ├── Settings/
│   │   │   └── JwtSettings.cs
│   │   ├── appsettings.Development.json
│   │   ├── appsettings.json
│   │   ├── MES.API.http
│   │   └── Program.cs
│   │
│   ├── MES.Application/
│   │   ├── Dependencies
│   │   ├── Common/
│   │   │   └── Result.cs
│   │   ├── DTOs/
│   │   │   ├── CreateWorkOrderDto.cs
│   │   │   ├── RecordProductionDto.cs
│   │   │   ├── WorkCentreDto.cs
│   │   │   └── WorkOrderDto.cs
│   │   ├── WorkCentres/
│   │   │   └── Queries/
│   │   │       └── GetWorkCentresQuery.cs
│   │   ├── WorkOrders/
│   │   │   ├── Commands/
│   │   │   │   ├── CompleteWorkOrderCommand.cs
│   │   │   │   ├── CreateWorkOrderCommand.cs
│   │   │   │   ├── RecordProductionCommand.cs
│   │   │   │   ├── ReleaseWorkOrderCommand.cs
│   │   │   │   └── StartWorkOrderCommand.cs
│   │   │   ├── Queries/
│   │   │   │   ├── GetWorkOrderByIdQuery.cs
│   │   │   │   └── GetWorkOrdersQuery.cs
│   │   │   └── Validators/
│   │   │       └── CreateWorkOrderValidator.cs
│   │   └── DependencyInjection.cs
│   │
│   ├── MES.Domain/
│   │   ├── Dependencies
│   │   ├── Common/
│   │   │   └── BaseEntity.cs
│   │   ├── Entities/
│   │   │   ├── ProductionEntry.cs
│   │   │   ├── QualityInspection.cs
│   │   │   ├── WorkCentre.cs
│   │   │   └── WorkOrder.cs
│   │   ├── Enums/
│   │   │   ├── InventoryTransactionType.cs
│   │   │   ├── QualityInspectionResult.cs
│   │   │   └── WorkOrderStatus.cs
│   │   ├── Exceptions/
│   │   │   └── DomainException.cs
│   │   └── Interfaces/
│   │       ├── IRepository.cs
│   │       ├── IUnitOfWork.cs
│   │       ├── IWorkCentreRepository.cs
│   │       └── IWorkOrderRepository.cs
│   │
│   ├── MES.Infrastructure/
│   │   ├── Dependencies
│   │   ├── Migrations/
│   │   ├── Persistence/
│   │   │   ├── Configurations/
│   │   │   │   └── WorkOrderConfiguration.cs
│   │   │   ├── Repositories/
│   │   │   │   ├── RepositoryBase.cs
│   │   │   │   ├── WorkCentreRepository.cs
│   │   │   │   └── WorkOrderRepository.cs
│   │   │   ├── MesDbContext.cs
│   │   │   └── UnitOfWork.cs
│   │   └── DependencyInjection.cs
│   │
│   └── MES.Web/
│       ├── Connected Services
│       ├── Dependencies
│       ├── Properties/
│       ├── wwwroot/
│       │   ├── css/
│       │   ├── lib/
│       │   ├── sample-data/
│       │   ├── favicon.png
│       │   ├── icon-192.png
│       │   └── index.html
│       ├── Layout/
│       │   ├── MainLayout.razor
│       │   ├── MainLayout.razor.css
│       │   ├── NavMenu.razor
│       │   └── NavMenu.razor.css
│       ├── Pages/
│       │   ├── WorkOrders/
│       │   │   ├── CreateWorkOrder.razor
│       │   │   └── WorkOrderList.razor
│       │   ├── Counter.razor
│       │   ├── Home.razor
│       │   └── Weather.razor
│       ├── Services/
│       │   ├── WorkCentreService.cs
│       │   └── WorkOrderService.cs
│       ├── Validation/
│       │   └── CustomValidationAttributes.cs
│       ├── _Imports.razor
│       ├── App.razor
│       └── Program.cs
│
└── tests/
    ├── MES.Application.Tests/
    │   └── WorkOrders/
    │       ├── CreateWorkOrderCommandHandlerTests.cs
    │       └── WorkOrderDomainTests.cs
    └── MES.Infrastructure.Tests/
```
## Prerequisites

| Tool | Version | Download |
|---|---|---|
| Visual Studio 2022 | 17.8+ | https://visualstudio.microsoft.com |
| .NET SDK | 9.0.x | https://dot.net |
| SQL Server Express | 2019+ | https://www.microsoft.com/sql-server |
| Git | Latest | https://git-scm.com |

**Visual Studio Workloads required:**
- ASP.NET and web development

**Verify .NET version:**
```powershell
dotnet --version
# 9.0.x
```

---


## Getting Started

### 1. Clone the repository

```powershell
git clone https://github.com/YourUsername/MES.Solution.git
cd MES.Solution
```
### 2. Restore packages

```powershell
dotnet restore
```
### 3. Set JWT Secret Key (User Secrets)

Right-click `MES.API` in Solution Explorer → **Manage User Secrets**, then paste:

```json
{
  "JwtSettings": {
    "SecretKey": ""
  }
}
```

Or via CLI:
```powershell
cd src/MES.API
dotnet user-secrets set "JwtSettings:SecretKey" ""
```

**Requirements for the secret key:**
- Minimum 32 characters
- Mix of uppercase, lowercase, numbers, and symbols
- Never commit the real value to Git
- Use a different key per environment (dev / staging / production)

**Generate a strong key using PowerShell:**
```powershell
$bytes = New-Object byte[] 32
[System.Security.Cryptography.RandomNumberGenerator]::Fill($bytes)
[Convert]::ToBase64String($bytes)
```
> The secret key must be at least 32 characters.
> Never put the real key in `appsettings.json`.

### 4. Apply database migrations

```powershell
# From solution root
dotnet ef migrations add InitialCreate `
    --project src/MES.Infrastructure `
    --startup-project src/MES.API `
    --context MesDbContext `
    --output-dir Persistence/Migrations

dotnet ef database update `
    --project src/MES.Infrastructure `
    --startup-project src/MES.API `
    --context MesDbContext
```
### 5. Run the application

**Visual Studio (recommended)**

1. Right-click Solution → **Properties**
2. Select **Multiple Startup Projects**
3. Set `MES.API` → **Start** (must be first)
4. Set `MES.Web` → **Start**
5. Press **F5**
---

## Configuration

### `appsettings.json` (safe to commit — no secrets)

```json
{
  "ConnectionStrings": {
    "MesDatabase": "Server=YourSQLServerName;Database=MesDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "JwtSettings": {
    "SecretKey": "",
    "Issuer": "MES.API",
    "Audience": "MES.Web",
    "ExpiryMinutes": 480
  }
}
```

### `secrets.json` (never committed — local machine only)

Path on Windows:
```
%APPDATA%\Microsoft\UserSecrets\<UserSecretsId>\secrets.json
```

Contents:
```json
{
  "JwtSettings": {
    "SecretKey": "YourSecretKeyMinimum32CharactersLong"
  }
}
```

### Port configuration

| Service | HTTPS | HTTP |
|---|---|---|
| MES.API | https://localhost:7100 | http://localhost:5100 |
| MES.Web | https://localhost:7200 | http://localhost:5200 |

If your ports differ, update these two locations:

**`MES.Web/Program.cs`**
```csharp
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7100")  // match MES.API port
});
```

**`MES.API/Program.cs`**
```csharp
builder.Services.AddCors(options =>
    options.AddPolicy("BlazorWasm", p =>
        p.WithOrigins("https://localhost:7200")      // match MES.Web port
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));
```
---

## Database Setup

### SQL Server Express (default)

```
Server=.\SQLEXPRESS;Database=MesDb;Trusted_Connection=true;TrustServerCertificate=true
```

### LocalDB alternative

```
Server=(localdb)\mssqllocaldb;Database=MesDb;Trusted_Connection=true;
```

### Seed data

Work centres are seeded automatically on first API startup.

| Code | Name | Department | Capacity/Shift |
|---|---|---|---|
| WC-001 | Assembly Line 1 | Assembly | 200 |
| WC-002 | Assembly Line 2 | Assembly | 200 |
| WC-003 | Welding Station | Fabrication | 150 |
| WC-004 | Paint Booth | Finishing | 100 |
| WC-005 | Quality Control | QC | 80 |
| WC-006 | Packaging Line | Packaging | 180 |

---

## Running the Application

### Application URLs

| Page | URL |
|---|---|
| Dashboard | https://localhost:7200 |
| Work Orders List | https://localhost:7200/workorders |
| New Work Order | https://localhost:7200/workorders/create |
| Swagger UI | https://localhost:7100 |

---
## API Documentation

Swagger UI available at `https://localhost:7100` in the Development environment.

## Project Descriptions

### MES.Domain
Pure C# with **zero external dependencies**. Contains all business entities, domain rules, enums, custom exceptions, and repository interfaces. All other layers depend on it — it depends on nothing.

- Private constructors + static `Create()` factory methods enforce valid initial state
- State-change methods (`Release()`, `Start()`, `Complete()`) enforce valid transitions
- `IReadOnlyCollection` on navigation properties prevents external mutation
- `BaseEntity` provides audit fields: `CreatedAt`, `UpdatedAt`, `CreatedBy`, `IsDeleted`

### MES.Application
Orchestrates business operations via **CQRS** and MediatR. Contains commands, queries, DTOs, AutoMapper profiles, and FluentValidation validators. Depends only on `MES.Domain`.

- `Result<T>` — wraps success/failure without throwing for expected cases
- Commands mutate state, Queries return read-only projections
- One command or query per file — strict single responsibility

### MES.Infrastructure
Implements all interfaces from `MES.Domain`. Contains EF Core DbContext, entity type configurations, repository implementations, Unit of Work, and seeding. The only layer that knows about SQL Server.

- `IEntityTypeConfiguration<T>` keeps mapping out of `DbContext`
- `ApplyConfigurationsFromAssembly()` auto-discovers all configurations
- Global query filters handle soft deletes automatically
- `RepositoryBase<T>` provides common CRUD; specific repos add domain queries

### MES.API
Thin ASP.NET Core Web API host. Controllers dispatch MediatR commands/queries and map results to HTTP responses. Configures JWT auth, Swagger, CORS, SignalR, and exception handling middleware.

### MES.Web
Blazor WebAssembly Standalone SPA. Communicates with `MES.API` via `HttpClient` (REST). Contains Razor pages, layout components, typed service classes, and custom validation attributes.

**Pages:**
- `Home.razor` — KPI dashboard with recent work orders
- `Pages/WorkOrders/WorkOrderList.razor` — list with filter, status badges, action buttons
- `Pages/WorkOrders/CreateWorkOrder.razor` — create form with full client-side validation

---

