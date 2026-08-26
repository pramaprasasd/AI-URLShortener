# Brownfield Scenario

Starting condition:
The initial redirect implementation performs a SQL lookup for every request.

Enhancement:
Improve redirect performance.

Analysis:
Short codes are read-heavy and frequently repeated.

Change:
Introduce cache-aside behavior with IMemoryCache.

Validation:
- Functional redirect tests
- Expiration behavior tests
- Review cache key strategy
- Review stale-data behavior

Trade-off:
IMemoryCache is process-local. A multi-instance production deployment should use Redis or another distributed cache.
