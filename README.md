# Openpay Platform - Settlements API (PC2)

## Overview

**Openpay Platform** is an ASP.NET Core RESTful API that supports the **settlement and fund transfer** operations of
[Openpay Peru](https://www.openpay.pe/), the payment gateway and technology platform of the BBVA group.

The API lets an operator register **Disbursement Orders**: instructions to transfer funds to an affiliate store within
a scheduled date interval. In this practical test the API exposes a single operation (`POST`) on the
`/api/v1/disbursement-orders` endpoint, and applies every business rule of the case.

## Author

| Field | Value |
|-------|-------|
| **Name** | Sebastian Lopez |
| **Student code** | U20241F946 |
| **Course** | Aplicaciones Web (1ASI0730) |
| **NRC** | 8168 |
| **Assessment** | Practical Test 2 (PC2) |
| **Solution** | `pc28168u20241f946` |
| **Project** | `PC28168.U20241F946.Openpay.Platform` |

## Technologies

- C# 14 and .NET 10 with ASP.NET Core (controllers)
- Entity Framework Core 10 with MySQL (`MySql.EntityFrameworkCore`), schema `openpay_settlements`
- Built-in OpenAPI document generation and [Scalar](https://scalar.com/) as interactive documentation
- Localization with `.resx` resources (`en`, `en-US`, `es`, `es-PE`)
- Humanizer for pluralization of table names

## Architecture

The solution follows **Domain-Driven Design** with a **layered architecture** inside every bounded context, the
**CQRS** pattern (commands handled by command services), **resources** and **assemblers** for the REST contracts.

```
PC28168.U20241F946.Openpay.Platform
|-- Settlements                  Core business bounded context
|   |-- Domain                   Aggregate, value objects, commands, repository contracts
|   |-- Application              Command service, application errors
|   |-- Infrastructure           EF Core repository
|   `-- Interfaces/REST          Controller, resources, assemblers
|-- Shared                       Base elements reused by other bounded contexts
|   |-- Domain                   IAuditableEntity, IBaseRepository, IUnitOfWork, DateInterval, FinancialAmount
|   |-- Application              Result<TValue, TError> pattern
|   `-- Infrastructure           AppDbContext, BaseRepository, UnitOfWork, interceptor, naming conventions
`-- Resources                    Localized messages (SharedResource.*.resx)
```

| Bounded context | Contents |
|-----------------|----------|
| `Settlements` | `DisbursementOrder` (aggregate root), `OpenpayReference`, `StoreIdentifier`, `DisbursementStatus` |
| `Shared` | `DateInterval`, `FinancialAmount`, auditing, repository base, unit of work, naming conventions |

More details: [`docs/class-diagram.puml`](docs/class-diagram.puml) and
[`docs/business-rules.md`](docs/business-rules.md) (traceability of every business rule to the code).

## Domain model

`DisbursementOrder` (aggregate root, auditable)

| Attribute | Type | Rules |
|-----------|------|-------|
| `Id` | `int` | Auto-generated primary key |
| `OrderTrackingCode` | `OpenpayReference` (`Guid`) | Required. Must be unique |
| `AffiliateStoreId` | `StoreIdentifier` (`Guid`) | Required |
| `ExecutionSchedule` | `DateInterval` | `ScheduledTo` must be later than `ScheduledFrom`. `HasElapsed(referenceDate)` |
| `TransferCap` | `FinancialAmount` | `Amount >= 0`, `CurrencyCode` not null/blank, `MultiplyBy(factor)` |
| `Status` | `DisbursementStatus` | `Queued = 10` (default), `Processed = 11`, `Cancelled = 12`, `Failed = 13` |
| `User` | `string` | Required, not blank, max 20 characters |
| `Remarks` | `string` | Optional, max 85 characters |
| `CreatedAt` / `UpdatedAt` | `DateTimeOffset?` | Managed by the persistence layer (auditing) |

## Endpoint

### `POST /api/v1/disbursement-orders`

Registers a disbursement order. The `id` is generated when the order is stored. Value objects are received as
separated primitive values.

Request body:

```json
{
  "orderTrackingCode": "3f2b8c1e-5d47-4a9b-9c3e-1a2b3c4d5e6f",
  "affiliateStoreId": "9a8b7c6d-5e4f-4a3b-8c2d-1e0f9a8b7c6d",
  "scheduledFrom": "2026-12-01",
  "scheduledTo": "2026-12-15",
  "transferCapAmount": 1500.50,
  "transferCapCurrencyCode": "PEN",
  "status": "Queued",
  "user": "operator01",
  "remarks": "First settlement of December"
}
```

`status` and `remarks` are optional. When `status` is omitted the order is stored as `Queued`.

Successful response (`201 Created`):

```json
{
  "id": 1,
  "orderTrackingCode": "3f2b8c1e-5d47-4a9b-9c3e-1a2b3c4d5e6f",
  "affiliateStoreId": "9a8b7c6d-5e4f-4a3b-8c2d-1e0f9a8b7c6d",
  "scheduledFrom": "2026-12-01",
  "scheduledTo": "2026-12-15",
  "transferCapAmount": 1500.50,
  "transferCapCurrencyCode": "PEN",
  "status": "Queued",
  "user": "operator01",
  "remarks": "First settlement of December",
  "updatedAt": "2026-11-18T16:20:31.4120000+00:00"
}
```

Error response (`application/problem+json`, example with `Accept-Language: en`):

```json
{
  "title": "The request contains invalid values.",
  "status": 400,
  "detail": "Scheduled to must be a valid date (yyyy-MM-dd) later than scheduled from."
}
```

| HTTP status | Meaning |
|-------------|---------|
| `201 Created` | The order was created. The body contains the order, its generated `id` and `updatedAt` |
| `400 Bad Request` | A value is missing, malformed or breaks a business rule |
| `409 Conflict` | An order with the same `orderTrackingCode` already exists |
| `500 Internal Server Error` | Unexpected error |

### Localization (i18n)

Error and validation messages follow the `Accept-Language` request header. Supported cultures: `en`, `en-US`
(default), `es` and `es-PE`. The OpenAPI documentation is always written in English.

## Database

- Engine: MySQL. Schema: `openpay_settlements`.
- Table `disbursement_orders` (plural, `snake_case`, primary key `id`). The schema and the table are created on startup.
- Unique index on `order_tracking_code`: duplicates are rejected by the application and by the database.

## How to run

1. Install the **.NET 10 SDK** and start a **MySQL** server.
2. Adjust the connection string in `PC28168.U20241F946.Openpay.Platform/appsettings.Development.json`
   (database `openpay_settlements`).
3. Run the API:

   ```bash
   dotnet run --project PC28168.U20241F946.Openpay.Platform
   ```

4. Open `http://localhost:5128/scalar` to explore the API, or use the requests of the
   `PC28168.U20241F946.Openpay.Platform.http` file (it covers the success case and every error case).

## Scope

Out of scope for this practical test: CORS support, security and testing.
