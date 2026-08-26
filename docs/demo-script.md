# Interview Demo Script

## 1. Start
```bash
docker compose up --build
```

Open `/swagger`.

## 2. Greenfield
Create a URL and show:
- request validation
- returned short code
- redirect

## 3. Security
Try:
```json
{"originalUrl":"javascript:alert(1)"}
```
Show 400.

## 4. Duplicate alias
Create alias `demo`, then create it again.
Show 409.

## 5. Analytics
Follow the short URL multiple times.
Open analytics endpoint.

## 6. Brownfield
Explain that the initial SQL lookup was optimized with cache-aside.
Show Application service and unit test.

## 7. Ambiguous requirement
Open `docs/scenarios/ambiguous.md`.
Explain the assumptions and why analytics is non-critical.

## 8. AI engineering
Show:
- `.github/copilot-instructions.md`
- `docs/ai/ai-execution-log.md`
- `docs/ai/traceability-matrix.md`

## 9. Quality gates
Show GitHub Actions and explain:
Build -> Test -> Dependency review -> Docker build.

## 10. Close
State:
"Copilot accelerated bounded tasks. I owned the architecture, reviewed generated output, rejected unsafe suggestions, and used tests and quality gates before accepting changes."