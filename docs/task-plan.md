# Task Decomposition

## Phase 1 — Greenfield
- G-001 Requirement normalization
- G-002 Solution/layer setup
- G-003 SQL schema and constraints
- G-004 Domain entities
- G-005 Short-code generator
- G-006 Create URL API
- G-007 Redirect API
- G-008 Validation and ProblemDetails
- G-009 Unit tests
- G-010 Docker and Swagger

## Phase 2 — Brownfield
- B-001 Identify redirect SQL hot path
- B-002 Add cache-aside lookup
- B-003 Validate expiration/cache behavior
- B-004 Add performance documentation
- B-005 Review multi-instance trade-off

## Phase 3 — Ambiguous
- A-001 Identify analytics ambiguity
- A-002 Define click semantics
- A-003 Define privacy decision
- A-004 Implement click capture
- A-005 Implement analytics API
- A-006 Ensure analytics failure does not block redirect
- A-007 Validate analytics queries

## Phase 4 — Quality
- Q-001 Security review
- Q-002 Concurrency review
- Q-003 Test suite
- Q-004 CI
- Q-005 Documentation
- Q-006 Engineer sign-off

Dependencies:
G-003 -> G-004 -> G-006/G-007
G-007 -> B-001 -> B-002
G-007 -> A-004
A-004 -> A-005
All implementation -> Q-*