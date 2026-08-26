# Scenario 2 — Brownfield

## Starting point
The first implementation reads SQL Server for every redirect.

## Requirement
Improve redirect performance for frequently accessed links.

## Analysis
The redirect endpoint is read-heavy. Repeated database lookups are unnecessary when the destination is stable.

## Change
Introduce cache-aside lookup with `IMemoryCache`.

## Engineer review
Copilot suggested cache placement and patterns. The engineer retained the cache only around the read path and kept SQL Server as the source of truth.

## Risks
- Process-local cache does not synchronize across instances.
- Expiration can create stale entries.

## Mitigation
- Cache duration is bounded.
- Expiration is checked after cache retrieval.
- Production multi-instance deployment should use distributed cache.

## Validation
- Cache hit/miss unit test.
- Expiration unit test.
- Manual redirect test.