# Scenario 1 — Greenfield

## Requirement
Create a short URL for a valid destination and redirect users to it.

## Acceptance criteria
- Valid HTTP/HTTPS URL returns 201.
- Returned short code is unique.
- Redirect returns 302 to the stored destination.
- Unknown code returns 404.
- Invalid destination returns 400.
- Duplicate custom alias returns 409.

## AI-assisted execution
Copilot was used for initial scaffolding, DTOs, service candidates and test suggestions.

## Engineer decisions
- Keep controllers thin.
- Put business validation in Application.
- Enforce uniqueness in SQL Server.
- Use cryptographically strong random bytes for identifiers.

## Validation
- Build
- Unit tests
- API/manual Swagger testing
- Database constraint review