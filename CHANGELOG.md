# Changelog

All notable changes of this solution. Author: Sebastian Lopez (U20241F946).

## [1.0.0] - 2026-11-18

### Added
- `POST /api/v1/disbursement-orders` endpoint with all the business rules of the Openpay case.
- `Settlements` bounded context (aggregate, value objects, command service, repository, REST interface).
- `Shared` bounded context (base repository, unit of work, auditing, `Result` pattern, `DateInterval`, `FinancialAmount`).
- MySQL persistence in schema `openpay_settlements` with a unique index on the order tracking code.
- Localized messages (`en`, `en-US`, `es`, `es-PE`) selected with the `Accept-Language` header.
- OpenAPI documentation in English with general API information and Scalar explorer.
- Documentation: `README.md`, `docs/business-rules.md` and `docs/class-diagram.puml`.
