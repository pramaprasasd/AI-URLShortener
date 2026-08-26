# ADR-003: Analytics Is Best Effort

## Decision
Analytics persistence failures must not prevent a successful redirect.

## Why
Redirect is the primary customer outcome. Analytics is secondary.

## Trade-off
A click can be lost during an analytics outage. A future event-driven ingestion pipeline can improve durability.