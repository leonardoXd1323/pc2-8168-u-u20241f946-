# Business rules traceability

Author: Sebastian Lopez (U20241F946). Every business rule of the Openpay case, the place of the code where it is
enforced and the outcome when it is broken. Paths are relative to `PC28168.U20241F946.Openpay.Platform/`.

| # | Business rule | Where it is enforced | Outcome |
|---|---------------|----------------------|---------|
| 1 | Two disbursement orders cannot share the same `OrderTrackingCode` | `Settlements/Application/Internal/CommandServices/DisbursementOrderCommandService.cs` (lookup by tracking code) and unique index in `AppDbContext` | `409 Conflict` |
| 2 | `OpenpayReference` is a value object with a `Guid` `Value` | `Settlements/Domain/Model/ValueObjects/OpenpayReference.cs` | Invalid GUID text: `400` |
| 3 | `StoreIdentifier` is a value object with a `Guid` `Value` | `Settlements/Domain/Model/ValueObjects/StoreIdentifier.cs` | Invalid GUID text: `400` |
| 4 | `DateInterval` holds `ScheduledFrom` and `ScheduledTo` (`DateOnly`) | `Shared/Domain/Model/ValueObjects/DateInterval.cs` | Invalid date text: `400` |
| 5 | `ScheduledTo <= ScheduledFrom` raises `ArgumentException` | `DateInterval` constructor | `400 Bad Request` |
| 6 | `HasElapsed(DateOnly referenceDate)` is true when `referenceDate > ScheduledTo` | `DateInterval.HasElapsed` | n/a (domain behavior) |
| 7 | `Amount >= decimal.Zero`, otherwise `ArgumentOutOfRangeException` | `Shared/Domain/Model/ValueObjects/FinancialAmount.cs` | `400 Bad Request` |
| 8 | `CurrencyCode` not null (`ArgumentNullException`) nor blank (`ArgumentException`) | `FinancialAmount` constructor | `400 Bad Request` |
| 9 | `MultiplyBy(decimal factor)` returns a new `FinancialAmount` with the same currency | `FinancialAmount.MultiplyBy` | n/a (domain behavior) |
| 10 | `DisbursementStatus`: `Queued = 10` (default), `Processed = 11`, `Cancelled = 12`, `Failed = 13` | `Settlements/Domain/Model/ValueObjects/DisbursementStatus.cs`, default applied in the `DisbursementOrder` constructor | Unknown status: `400` |
| 11 | `User` is required (not null, not blank) and at most 20 characters | `DisbursementOrder` constructor and `[Required]` in the create resource | `400 Bad Request` |
| 12 | `Remarks` is optional and at most 85 characters | `DisbursementOrder` constructor | `400 Bad Request` |
| 13 | Persisted information (required fields, auto-generated key) | `Shared/Infrastructure/Persistence/EFC/Configuration/AppDbContext.cs` (Fluent API) | n/a |
| 14 | Response fields: id, orderTrackingCode, affiliateStoreId, scheduledFrom, scheduledTo, transferCapAmount, transferCapCurrencyCode, status, user, remarks, updatedAt | `Settlements/Interfaces/REST/Resources/DisbursementOrderResource.cs` and `DisbursementOrderResourceFromEntityAssembler.cs` | `201 Created` |
| 15 | The aggregate root is auditable (creation and last update dates) | `DisbursementOrderAudit.cs` (partial class), `IAuditableEntity`, `AuditableEntityInterceptor` | n/a |
| 16 | Responses support localization (i18n) | `Resources/SharedResource.*.resx`, request localization in `Program.cs` | Messages in `en`, `en-US`, `es`, `es-PE` |

## Technical constraints

| Constraint | Evidence |
|------------|----------|
| C# 14, .NET 10, ASP.NET Core | `PC28168.U20241F946.Openpay.Platform.csproj` and `global.json` |
| Solution `pc28168u20241f946`, project `PC28168.U20241F946.Openpay.Platform` | Solution and project file names |
| Bounded contexts `Settlements` and `Shared` | Top-level folders |
| `DateInterval` and `FinancialAmount` in `Shared` | `Shared/Domain/Model/ValueObjects` |
| MySQL, schema `openpay_settlements` | `appsettings*.json` |
| `Accept-Language` (EN, EN-US, ES, ES-PE) | `Program.cs` request localization |
| DDD, layered architecture, CQRS, Resource, Assembler | Folder structure of each bounded context |
| Fluent API for persistence concepts | `AppDbContext` |
| kebab-case URLs | `KebabCaseRouteNamingConvention` |
| Properties for entity attributes | `DisbursementOrder` |
| XML documentation with `<remarks>` and author | All source files |
| OpenAPI documentation in English | `DisbursementOrdersController`, `OpenApiDocumentTransformers` |
| Exception management | `ProblemDetails`, `UseExceptionHandler`, controller `try/catch`, `Result<TValue, TError>` |
