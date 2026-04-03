# Run end-to-end tests: Playwright will automatically start servers via webServer config

# Determine project root
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir

Set-Location "$ProjectRoot\client\e2e-tests"

Write-Host "Starting Tailspin Toys E2E Tests" -ForegroundColor Blue
Write-Host "Starting servers..." -ForegroundColor Green
Write-Host "  * ASP.NET Core API server: http://localhost:5100"
Write-Host "  * Blazor client server: http://localhost:4321"
Write-Host ""
Write-Host "Running tests:" -ForegroundColor Blue

# Run Playwright tests - this will automatically start the servers silently
npx playwright test

# Store and return the exit code
$TestExitCode = $LASTEXITCODE

Write-Host ""
Write-Host "E2E tests completed with exit code: $TestExitCode" -ForegroundColor Blue
exit $TestExitCode
