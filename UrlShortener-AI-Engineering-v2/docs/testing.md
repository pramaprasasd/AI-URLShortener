# Testing Strategy

## Unit tests
Business rules:
- valid URL creation
- invalid schemes
- duplicate aliases
- expiration
- cache behavior
- short-code properties

## Integration tests
The next production step is to add API-to-SQL Server tests using an isolated SQL Server container. The current CI keeps the fast unit suite as the baseline.

## Security tests
- reject javascript/data/file schemes
- reject oversized URLs
- reject invalid custom aliases
- ensure errors do not expose stack traces

## Concurrency tests
The database unique constraint must be tested with concurrent custom-alias creation.

## Quality gates
- dotnet restore
- dotnet build
- dotnet test
- dependency vulnerability review
- Docker build
- human code review

## Test ownership
AI can propose tests. The engineer determines whether the tests prove the actual requirement and edge cases.