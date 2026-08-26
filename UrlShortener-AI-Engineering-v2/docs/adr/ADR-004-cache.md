# ADR-004: Cache Aside

## Decision
Use cache-aside lookup for short-code resolution.

## Why
The redirect path is read-heavy and destinations change infrequently.

## Trade-off
IMemoryCache is process-local. Distributed cache is required for coordinated multi-instance production.