# ADR-001: Modular Monolith

## Decision
Use a modular monolith with API, Application, Domain and Infrastructure projects.

## Why
The assignment is a prototype and does not require independent deployment of services. This reduces operational complexity while retaining boundaries.

## Revisit when
- team ownership requires independent deployment;
- redirect and analytics have materially different scaling profiles;
- deployment frequency or fault isolation requires service separation.