# Architecture Overview

The prototype is a modular monolith.

Client -> ASP.NET Core API -> Application -> Domain/Infrastructure -> SQL Server

Cross-cutting concerns include validation, logging, caching and health checks.

## Why modular monolith?
The assessment requires production-grade engineering judgment. A modular monolith keeps deployment and operational complexity low while preserving clear boundaries. The architecture can be split into services later if scale or team boundaries justify it.

## Key decisions
1. SQL Server is the source of truth.
2. ShortCode has a database unique constraint.
3. Redirect is the critical path.
4. Analytics is best-effort and must not prevent redirect.
5. IMemoryCache is used for the prototype; Redis can replace it for multi-instance deployments.
