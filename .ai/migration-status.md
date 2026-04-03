# Tailspin Toys Workshop Migration: Node/Python → .NET 10

## Status: ✅ COMPLETE — All phases done, CI green, visual parity confirmed

## Final Validation (April 2026)

### Tests
- ✅ 12/12 backend xUnit tests passing
- ✅ 21/21 E2E C# Playwright tests passing
- ✅ CI fully green (deploy + run-tests workflows)

### Visual Parity (side-by-side comparison)
- ✅ Home page matches original
- ✅ Game Details page matches original
- ✅ About page matches original
- ✅ Header/navigation matches original

### Structure Validation
- ✅ All English workshop files (10) match original headings/structure
- ✅ All GitHub config files adapted (agents, instructions, skills, workflows)
- ✅ All scripts present (setup-env, start-app, run-server-tests, run-e2e-tests × .sh/.ps1)
- ✅ DevContainer configured for .NET 10
- ✅ Docs site files present (English only)
- ✅ Seed data (games.csv) identical to original
- ✅ All branch refs updated to dotnet-workshop
- ✅ No stale Python/Flask/Svelte/Astro references

### Intentional Differences from Original
- Localized content (es/, pt_BR/) excluded per user request (English only)
- Python instruction files replaced with .NET equivalents (blazor, aspnetcore-endpoint, dotnet-tests)
- venv/ not included (not needed for .NET)
- TailspinToys.slnx added (.NET solution file)
- GameCard badges not shown (matches original behavior — original code references flat properties that don't exist in API response)

## Architecture Mapping

| Original | .NET Equivalent |
|----------|----------------|
| Flask (Python) backend | ASP.NET Core Minimal API |
| SQLAlchemy ORM | Entity Framework Core |
| SQLite | SQLite (same) |
| Astro/Svelte frontend | Blazor Interactive Server |
| Tailwind CSS | Tailwind CSS via CDN |
| Python unittest | xUnit |
| Playwright TS e2e tests | Playwright C# xUnit e2e tests |
| npm scripts | dotnet CLI + shell scripts |
| Flask blueprints | Minimal API route groups |
| pip/venv | dotnet restore/NuGet |

## Key Technical Decisions
- Blazor Interactive Server (not WASM) — uses SignalR for interactivity
- Tailwind CSS via CDN `<script src="https://cdn.tailwindcss.com">` — no Node dependency
- EF Core InMemory for test isolation, SQLite for production
- Auto-seed database in Program.cs on startup
- Random(42) fixed seed for consistent star ratings
- Ports: API on 5100, Frontend on 4321 (matching original)
- @rendermode InteractiveServer required on all interactive components

## Key Issues Resolved
1. WebApplicationFactory + EF Core: filter ALL DbContext services by FullName
2. Database path: use ContentRootPath not AppContext.BaseDirectory
3. Blazor @rendermode: MUST be on every component with event handlers
4. Blazor Server timing: 15s assertion timeout, 30s navigation for CI
5. CI E2E: run-e2e-tests.sh auto-starts/stops servers
6. GameCard badges: original never renders them (flat property bug)
7. Header outside-click: inline JS with suppress-error="BL9992"
8. SeedDatabase guard: checks IsSqlite() to skip in InMemory tests

## Learnings
- Original publishers.py routes file is EMPTY (placeholder)
- Original has filtering.spec.ts referenced but doesn't exist
- ui.instructions.md referenced but doesn't exist
- GameCard uses publisher_name/category_name flat properties that don't exist in API
- GameDetails uses nested publisher/category objects
- API returns starRating (camelCase) not star_rating
