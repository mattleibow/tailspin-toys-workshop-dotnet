# Setup environment: .NET restore

# Determine project root
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectRoot = Split-Path -Parent $ScriptDir

Set-Location $ProjectRoot

Write-Host "Restoring .NET dependencies..."
dotnet restore

Write-Host "Environment setup complete."
