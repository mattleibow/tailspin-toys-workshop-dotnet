# Discrepancy Report: Node/Python vs .NET Workshop

## Comparison Date: 2026-04-03

## Summary
Three parallel comparison agents reviewed every file in both repos. All identified
discrepancies have been resolved in the latest commit.

Final follow-up sweep also resolved the remaining parity nits around docs links,
launch settings, setup scripts, dark theme activation, and closer test assertions.

---

## Workshop Docs (10 files)
✅ All 10 files have identical structure, heading counts, step counts, tip boxes, and images.
✅ No stale Python/Flask/Svelte/Astro references found.
- Fixed: `03-mcp.md` was missing the Playwright row in the MCP registration table.

## GitHub Config
✅ copilot-instructions.md — all sections present, repo structure accurate
✅ dependabot.yml — properly adapted for NuGet
✅ All 3 workflows — correct branch refs (dotnet-workshop), proper .NET steps
- Fixed: run-tests.yml was missing Playwright report artifact upload
✅ All 3 agents — now match original detail level exactly
- Fixed: accessibility.md was skeletal → replaced with full WCAG 2.1 content
- Fixed: code-review.md was skeletal → replaced with full review checklist
- Fixed: seo-agent.md was skeletal → copied original (tech-agnostic)
✅ Both skills — content matches original depth
- Fixed: testing-guidelines referenced stale `client/e2e-tests/` path
- Fixed: branches-commits-prs was simplified → replaced with full original
✅ 5 instruction files — properly adapted for .NET
✅ .vscode config — properly adapted
✅ .devcontainer — .NET 10 + Node (for Copilot CLI) + SQLite + Playwright

## Application Code
✅ Same API endpoints: GET /api/games, GET /api/games/{id}
✅ Same JSON response shape (camelCase starRating, nested publisher/category)
✅ Same seed data: games.csv identical
✅ Same pages: Home (/), About (/about), Game Details (/game/{id})
✅ Same data-testid attributes on all elements
- Fixed: EmptyState text had extra "Check back soon!" → removed
- Fixed: GameCard was truncating descriptions in C# → use CSS line-clamp only
- Fixed: Star rating logic used different thresholds → matched original exactly
- Fixed: Header was missing outside-click-to-close behavior → added JS handler

## Tests
✅ 12 backend xUnit tests — match original 12 Python tests
✅ 21 E2E Playwright tests (C#) — match original 21 TypeScript tests
  - HomeTests: 4 tests (title, heading, branding, welcome)
  - GamesTests: 6 tests (listing, navigation, details, button, back, 404)
  - AccessibilityTests: 11 tests (axe WCAG, keyboard nav, focus, ARIA, contrast, semantic, SVGs)

## Remaining Intentional Differences
These are expected differences between the tech stacks:

1. **Framework**: Flask/SQLAlchemy → ASP.NET Core/EF Core
2. **Frontend**: Astro/Svelte → Blazor Interactive Server  
3. **Tailwind delivery**: npm build → CDN (no Node dependency for frontend)
4. **Test framework**: Python unittest → xUnit; TypeScript Playwright → C# Playwright
5. **DB seeding**: Python script → auto-seeds in Program.cs on startup
6. **Random seed**: Python uses random.random() → .NET uses Random(42) for consistent ratings
7. **Localized content**: es/pt_BR translations excluded (English only per requirements)
8. **Menu behavior**: Original uses client-side JS toggle; .NET uses Blazor server + JS hybrid
