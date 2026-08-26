# Requirement Understanding

## Normalized problem

Build a reliable URL shortening service that creates a unique short identifier for a destination URL, redirects users through that identifier, and records basic click analytics without allowing analytics failures to break the redirect path.

## Functional requirements

1. Create a short URL from an HTTP/HTTPS destination.
2. Support optional custom aliases.
3. Ensure short codes are unique.
4. Redirect a valid short code.
5. Reject unknown and expired short codes.
6. Record click events.
7. Provide aggregate analytics.
8. Return predictable API errors.

## Non-functional requirements

- Maintainable C# architecture.
- SQL Server persistence.
- Safe concurrent creation.
- Low-latency redirect path.
- Secure input handling.
- Automated tests.
- CI quality gates.
- Engineer-controlled AI assistance.

## Explicit assumptions

- HTTP and HTTPS are allowed.
- Custom aliases are case-sensitive.
- Short codes are 7 characters by default.
- Analytics are eventually consistent.
- Bot detection is out of scope.
- IP is hashed rather than stored as raw data.
- The MVP has no user authentication.
- Multi-instance distributed caching is a production follow-up.