# Interviewer Assessment Summary

## What this repository demonstrates

### Engineer-led AI execution
GitHub Copilot is configured with repository instructions and used inside bounded tasks. The engineer reviews all material output.

### Greenfield
Core URL creation and redirect were designed and implemented from a normalized requirement.

### Brownfield
A SQL-backed redirect path is improved with cache-aside behavior while preserving correctness.

### Ambiguous
The vague analytics requirement is decomposed into explicit questions, assumptions, acceptance criteria and trade-offs.

### Validation
Build, tests, dependency review, Docker build, security review and human approval are treated as quality gates.

### Production judgment
The repository deliberately documents what is implemented now and what would change at production scale.

## Key interview talking points

1. Database uniqueness is the final concurrency guard.
2. Analytics is non-critical to redirect availability.
3. Cache is an optimization, not the source of truth.
4. Input validation is applied before persistence.
5. AI suggestions are reviewed rather than blindly accepted.
6. The architecture avoids premature microservices.
7. Known limitations are explicit and defensible.
