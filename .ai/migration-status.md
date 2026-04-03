# Tailspin Toys Workshop Migration: Node/Python → .NET 10

## Status: ✅ COMPLETE (Phase 2 - Full .NET conversion)

## Validation Results
- ✅ All 12 backend tests passing (xUnit)
- ✅ API live-tested: 21 games seeded, GET /api/games and GET /api/games/{id} working
- ✅ Blazor frontend builds and runs on port 4321
- ✅ All 10 workshop files (English) created and adapted
- ✅ 14 workshop images copied
- ✅ All 8 scripts created (4 .sh + 4 .ps1)
- ✅ All GitHub config files present (copilot-instructions, agents, skills, workflows, etc.)
- ✅ All data-testid attributes match for e2e tests
- ✅ Seed data (games.csv) byte-identical to original
- ✅ Docs site files present
- ✅ No stale Python/Flask/Svelte/Astro references in workshop content
- ✅ DevContainer configured for .NET 10
- ✅ **E2E tests converted to C#/xUnit with Microsoft.Playwright** (Phase 2)
- ✅ **All workflows updated to reference dotnet-workshop branch** (Phase 2)
- ✅ **GitHub Pages deploy workflow uses dotnet build verification** (Phase 2)
- ✅ **All npm/Node.js removed from workflows** (except Copilot CLI install) (Phase 2)
- ✅ **AGENTS.md and copilot-instructions.md updated for C# E2E tests** (Phase 2)
- ✅ **Playwright instruction file rewritten for C#** (Phase 2)
- ✅ **Solution file includes all 4 projects** (Phase 2)

## Decisions Made
- Blazor Interactive Server (not WASM) for frontend
- Tailwind CSS via CDN (no Node dependency)
- E2E tests kept as TypeScript/Playwright (language-agnostic UI tests)
- Localized content (es/, pt_BR/) excluded per user request (English only)
- API on port 5100, Frontend on port 4321 (matching original)

## Architecture Mapping

| Original | .NET Equivalent |
|----------|----------------|
| Flask (Python) backend | ASP.NET Core Minimal API |
| SQLAlchemy ORM | Entity Framework Core |
| SQLite | SQLite (same) |
| Astro/Svelte frontend | Blazor (Interactive Server or WASM) |
| Tailwind CSS | Tailwind CSS (via CDN or build) |
| Python unittest | xUnit |
| Playwright e2e tests | Playwright e2e tests (same) |
| npm scripts | dotnet CLI + scripts |
| Flask blueprints | Minimal API route groups |
| pip/venv | dotnet restore/NuGet |

## Key Decisions
- Use .NET 10 (preview/latest)
- Blazor Server (Interactive Server) for frontend - uses C# and can still use JS interop for menu
- Tailwind CSS via CDN for simplicity (no Node dependency)
- Entity Framework Core with SQLite
- xUnit for backend tests
- Playwright for e2e tests (C# Playwright or keep TypeScript Playwright)
- Keep same ports: API on 5100, Frontend on 4321 (or adjust in scripts)
- Keep same data: games.csv seed data identical
- Keep same API shape: /api/games, /api/games/{id}

## File Mapping

### Root Files (copy as-is or adapt)
- [x] README.md - adapt for .NET
- [ ] AGENTS.md - adapt for .NET
- [x] CODE_OF_CONDUCT.md - copy as-is
- [x] CONTRIBUTING.md - copy as-is  
- [x] LICENSE - copy as-is
- [x] SECURITY.md - copy as-is
- [x] SUPPORT.md - copy as-is
- [x] .gitignore - adapt for .NET
- [x] .gitattributes - copy as-is

### Server → Backend API
- [ ] server/app.py → src/TailspinToys.Api/Program.cs
- [ ] server/models/ → src/TailspinToys.Api/Models/
- [ ] server/routes/ → src/TailspinToys.Api/Routes/ (or endpoints)
- [ ] server/utils/database.py → EF Core DbContext
- [ ] server/utils/seed_database.py → Data seeding in DbContext
- [ ] server/utils/seed_data/games.csv → same file
- [ ] server/tests/ → tests/TailspinToys.Api.Tests/
- [ ] server/requirements.txt → .csproj PackageReferences

### Client → Blazor Frontend
- [ ] client/src/pages/ → src/TailspinToys.Web/Components/Pages/
- [ ] client/src/components/ → src/TailspinToys.Web/Components/
- [ ] client/src/layouts/ → src/TailspinToys.Web/Components/Layout/
- [ ] client/src/styles/ → wwwroot/css/
- [ ] client/src/types/ → shared models
- [ ] client/src/middleware.ts → API proxy (HttpClient in Blazor)
- [ ] client/e2e-tests/ → tests/TailspinToys.E2E/
- [ ] client/playwright.config.ts → playwright.config.ts (adapted)

### Scripts
- [ ] scripts/setup-env.sh/.ps1 → dotnet restore
- [ ] scripts/start-app.sh/.ps1 → dotnet run (both projects)
- [ ] scripts/run-server-tests.sh/.ps1 → dotnet test
- [ ] scripts/run-e2e-tests.sh/.ps1 → playwright test

### DevContainer
- [ ] .devcontainer/devcontainer.json → adapt for .NET

### GitHub Config
- [ ] .github/copilot-instructions.md → adapt for .NET/C#/Blazor
- [ ] .github/instructions/*.md → adapt all for .NET equivalents
- [ ] .github/agents/*.md → adapt for .NET
- [ ] .github/skills/ → adapt for .NET
- [ ] .github/workflows/ → adapt for .NET
- [ ] .github/dependabot.yml → adapt for NuGet
- [ ] .vscode/ → adapt for .NET

### Workshop Content
- [ ] workshop/README.md → adapt
- [ ] workshop/00-prereqs.md → adapt
- [ ] workshop/01-custom-instructions.md → adapt
- [ ] workshop/02-install-copilot-cli.md → keep mostly same
- [ ] workshop/03-mcp.md → adapt
- [ ] workshop/04-generating-code.md → adapt
- [ ] workshop/05-agent-skills.md → adapt
- [ ] workshop/06-custom-agents.md → adapt
- [ ] workshop/07-slash-commands.md → adapt
- [ ] workshop/08-review.md → adapt

### Docs Site
- [ ] docs/*.html → adapt references
- [ ] docs/*.css → copy as-is
- [ ] docs/*.js → copy as-is
- [ ] workshop/images/ → copy as-is

## Current Status
- Phase: PLANNING → BUILDING
- Next: Create project structure and implement backend API

## Learnings
- Original publishers.py routes file is EMPTY
- Original has filtering.spec.ts referenced but doesn't exist
- ui.instructions.md referenced but doesn't exist
- Only English needed (skip es/, pt_BR/ translations)
- GameCard uses publisher_name/category_name flat properties
- GameDetails uses nested publisher/category objects
- API returns starRating (camelCase) not star_rating
