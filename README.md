# URL Shortener — AI-Assisted Engineering Assessment

A production-oriented URL shortener prototype built with **C# / .NET 8 / ASP.NET Core / SQL Server / EF Core**, with **GitHub Copilot** used as an engineer-controlled accelerator.

> AI assists within bounded engineering tasks. The engineer owns architecture, correctness, security, maintainability, validation and production readiness.

## Assessment coverage

| Assessment area | Evidence |
|---|---|
| Requirement understanding | `docs/requirements.md` |
| Task decomposition | `docs/task-plan.md` |
| Greenfield | `docs/scenarios/greenfield.md` |
| Brownfield | `docs/scenarios/brownfield.md` |
| Ambiguous requirements | `docs/scenarios/ambiguous.md` |
| AI-assisted execution | `docs/ai/ai-execution-log.md` |
| Traceability | `docs/ai/traceability-matrix.md` |
| Architecture | `docs/architecture.md` |
| Security | `docs/security/threat-model.md` |
| Testing | `docs/testing.md` |
| Performance | `docs/performance.md` |
| Decisions | `docs/adr/` |
| Quality gates | `.github/workflows/ci.yml` |

## Features

- Create short URLs
- Cryptographically strong random short codes
- Custom aliases
- URL validation: HTTP/HTTPS only
- Optional expiration
- Fast redirect path with cache-aside lookup
- Click analytics
- Best-effort analytics so analytics failure does not break redirect
- Atomic click counter update
- SQL Server unique constraint for concurrency protection
- Rate limiting on URL creation
- RFC 7807-style ProblemDetails
- Health endpoint
- Swagger/OpenAPI
- Dockerized API + SQL Server
- Unit tests
- GitHub Actions CI
- Repository-level Copilot instructions

## Prerequisites

- .NET 8 SDK
- Docker Desktop
- Git

## Fastest run: Docker

```bash
docker compose up --build
```

Open:

- Swagger: http://localhost:8080/swagger
- Health: http://localhost:8080/health

Stop:

```bash
docker compose down
```

Reset SQL Server data:

```bash
docker compose down -v
```

## Local run

Start SQL Server:

```bash
docker compose up -d sqlserver
```

Then:

```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/UrlShortener.Api
```

For local development, the default connection string points to localhost SQL Server.

## API demo

### 1. Create

```http
POST http://localhost:8080/api/v1/urls
Content-Type: application/json

{
  "originalUrl": "https://www.microsoft.com",
  "customAlias": "microsoft",
  "expiresAtUtc": null
}
```

### 2. Redirect

```text
GET http://localhost:8080/r/microsoft
```

### 3. Analytics

```text
GET http://localhost:8080/api/v1/urls/1/analytics
```

## Example curl

```bash
curl -X POST http://localhost:8080/api/v1/urls \
  -H "Content-Type: application/json" \
  -d '{"originalUrl":"https://www.github.com","customAlias":null,"expiresAtUtc":null}'
```

Then use the returned `shortCode`:

```bash
curl -i http://localhost:8080/r/<shortCode>
```

## Engineering decisions

### Modular monolith
The system uses API, Application, Domain and Infrastructure boundaries. This gives clear separation without introducing unnecessary distributed-system complexity.

### SQL Server is the source of truth
The database enforces uniqueness for short codes. Application-level checks improve the user experience, but the database remains the final concurrency guard.

### Cache-aside
Redirects are read-heavy. `IMemoryCache` reduces repeated SQL lookups. For multi-instance production deployment, replace it with a distributed cache such as Redis.

### Analytics is non-critical
A successful redirect should not fail because analytics persistence is temporarily unavailable.

### Privacy
The prototype hashes the source IP address before storing it. Production retention, access controls and privacy requirements must still be explicitly approved.

## Production hardening still required

This is an assessment prototype, not a deployed production service. Before production:

- Use EF Core migrations and controlled database deployment instead of `EnsureCreated`.
- Use distributed cache for multiple API instances.
- Add authentication/authorization for management and analytics APIs.
- Add bot detection and abuse prevention.
- Add centralized metrics/tracing.
- Add data retention/partitioning strategy for large click-event volumes.
- Add managed secret storage.
- Add full integration tests against SQL Server.
- Add deployment-specific health/readiness behavior.
- Perform load, security and penetration testing.

## GitHub Copilot

The repository contains `.github/copilot-instructions.md`, which GitHub documents as the repository-wide custom-instructions mechanism for Copilot. citeturn0search0turn0search2

Recommended interview workflow:

1. Engineer interprets requirement.
2. Engineer creates a bounded task.
3. Engineer gives Copilot context, constraints and acceptance criteria.
4. Copilot proposes code/tests/docs.
5. Engineer reviews and modifies/rejects suggestions.
6. Automated quality gates run.
7. Engineer signs off.
8. Decision is recorded for material changes.

## Suggested interview demo

1. Start Docker Compose.
2. Open Swagger.
3. Create a short URL.
4. Follow the redirect.
5. Show analytics.
6. Show SQL Server tables/indexes.
7. Demonstrate an expired URL.
8. Demonstrate duplicate custom alias -> 409.
9. Demonstrate invalid `javascript:` URL -> 400.
10. Show cache behavior in the brownfield scenario.
11. Show Copilot instructions.
12. Show AI execution log and traceability matrix.
13. Show GitHub Actions.
14. Explain trade-offs and production limitations.
