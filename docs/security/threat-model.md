# Threat Model

## Open redirect / malicious destinations
Risk: attackers can use the service to create malicious destinations.

Mitigation:
- Only HTTP/HTTPS destinations are accepted.
- The service does not redirect based on arbitrary user-supplied redirect targets; it resolves stored short codes.

## Injection
Risk: SQL injection.

Mitigation:
- EF Core and parameterized interpolated SQL are used.
- No user input is concatenated into SQL.

## Abuse
Risk: automated URL creation.

Mitigation:
- Fixed-window rate limit on URL creation.
- Input length limits.
- Production follow-up: authentication, quotas, WAF and abuse detection.

## Sensitive data
Risk: request metadata leakage.

Mitigation:
- IP address is hashed before persistence.
- Logs avoid sensitive payloads.

## Error disclosure
Risk: internal exceptions exposed to clients.

Mitigation:
- Global exception handler returns ProblemDetails without stack traces.

## Secrets
Never commit database passwords or API keys. Docker Compose uses an environment variable with a development fallback solely for local assessment execution.