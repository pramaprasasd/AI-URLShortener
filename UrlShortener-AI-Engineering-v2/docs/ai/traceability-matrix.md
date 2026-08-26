# AI / Requirement Traceability Matrix

| ID | Requirement | Task | AI assistance | Engineer decision | Validation |
|---|---|---|---|---|---|
| R-01 | Create short URL | G-006 | Service/controller scaffold | Kept business logic out of controller | Unit/API |
| R-02 | Unique code | G-005 | Generator candidate | Used cryptographic RNG + DB constraint | Unit/DB |
| R-03 | Redirect | G-007 | Endpoint scaffold | Analytics is non-blocking | API |
| R-04 | Expiration | G-008 | Test suggestions | Explicit future-time validation | Unit |
| R-05 | Analytics | A-004/A-005 | Query/test candidates | Event + aggregate counter | Query/API |
| R-06 | Reliability | A-006 | Failure scenario review | Redirect survives analytics failure | Review/test |
| R-07 | Performance | B-002 | Cache pattern | IMemoryCache prototype | Unit/design |
| R-08 | Security | Q-001 | Threat suggestions | HTTP(S), limits, ProblemDetails | Security review |
| R-09 | AI governance | Q-006 | Documentation draft | Engineer sign-off required | Evidence review |