# Performance and Scalability

## Redirect path
1. Cache lookup
2. SQL lookup on cache miss
3. Redirect
4. Best-effort analytics

## Database
- Unique index on ShortCode
- Composite index on ClickEvents(ShortUrlId, ClickedAtUtc)
- Atomic ClickCount update

## Expected bottlenecks
- SQL reads during cache misses
- ClickEvents growth
- Analytics aggregation

## Scale-out path
For multiple API instances:
- replace IMemoryCache with Redis;
- consider asynchronous event ingestion for click events;
- partition/archive ClickEvents;
- pre-aggregate analytics for high-volume reporting.

## Trade-off
The prototype favors simplicity and explainability over premature distributed infrastructure.