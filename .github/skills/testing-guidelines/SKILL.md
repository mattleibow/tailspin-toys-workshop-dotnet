# Testing Guidelines Skill

This skill provides guidance on writing and running tests for the Tailspin Toys project.

## Backend Tests (xUnit)

- Tests are in `server/TailspinToys.Api.Tests/`
- Run with: `dotnet test server/TailspinToys.Api.Tests/`
- Use WebApplicationFactory for integration tests
- Use InMemoryDatabase for test isolation

## E2E Tests (Playwright)

- Tests are in `client/e2e-tests/`
- Run with: `./scripts/run-e2e-tests.sh`
- Use `data-testid` attributes for element selection
- Use `test.step()` for logical grouping
- Never use hard-coded waits

## Test Data

- Backend tests use in-memory test data defined as class constants
- E2E tests run against a seeded SQLite database

## Before Committing

1. Run backend tests: `./scripts/run-server-tests.sh`
2. Run E2E tests: `./scripts/run-e2e-tests.sh`
3. Verify no regressions in existing tests
