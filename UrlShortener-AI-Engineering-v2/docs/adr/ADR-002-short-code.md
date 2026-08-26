# ADR-002: Random Short Codes

## Decision
Generate 7-character Base62 identifiers using cryptographically strong random bytes.

## Why
Avoid predictable sequential IDs and keep codes compact.

## Constraint
The database unique index remains the authoritative collision guard.