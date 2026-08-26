# Scenario 3 — Ambiguous Requirement

## Requirement
"Provide analytics showing how many people clicked each link."

## Ambiguities
- What counts as a click?
- Are bots included?
- What is a unique visitor?
- Should IP addresses be retained?
- What is the retention period?
- Does analytics need to be real-time?
- What happens when analytics persistence fails?

## Decisions
- One successful redirect attempts one click event.
- Bot filtering is out of scope.
- Unique visitors are not claimed by the MVP.
- Analytics are eventually consistent.
- IP addresses are hashed before persistence.
- Analytics failure does not fail redirect.

## Engineering judgment
The redirect is the customer-critical path. Analytics is secondary. This separation improves reliability and allows analytics infrastructure to evolve independently.