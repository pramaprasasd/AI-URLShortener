# AI-Assisted Engineering Execution Log

The project is intentionally designed around engineer-led AI assistance.

## G-005 — Short-code generator
**Prompt intent:** implement a 7-character collision-resistant identifier.

**Copilot proposal:** random identifier implementation.

**Engineer review:** replaced predictable random generation with `RandomNumberGenerator`.

**Reason:** identifiers should not depend on a predictable PRNG.

**Validation:** unit tests + code review.

**Decision:** Accepted after modification.

## G-006 — Create URL service
**Prompt intent:** implement the create use case with validation and persistence.

**Engineer constraints:** HTTP/HTTPS only, unique custom alias, future expiration, SQL constraint as final guard.

**Decision:** Accepted after refactoring into Application service.

## G-007 — Redirect
**Prompt intent:** resolve code and redirect.

**Engineer constraint:** analytics must not break redirect.

**Decision:** Accepted after adding best-effort analytics handling.

## B-002 — Cache
**Prompt intent:** improve read-heavy redirect path.

**Copilot proposal:** cache-aside.

**Engineer review:** retained bounded cache and explicit expiration check.

**Trade-off:** process-local cache is acceptable for prototype, not final multi-instance production.

## A-004 — Analytics
**Prompt intent:** record clicks without making analytics part of redirect correctness.

**Engineer decision:** store hashed IP and request metadata, increment aggregate counter atomically.

## Q-001 — Security
Copilot was used as a review assistant to identify malformed URLs, unsafe schemes, oversized inputs and error disclosure risks.

**Engineer ownership:** every finding was manually evaluated before implementation.