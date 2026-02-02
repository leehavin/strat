#!/usr/bin/env pwsh
# Build script for Windows PowerShell

param(
    [Parameter()]
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    
    [Parameter()]
    [switch]$Clean,
    
    [Parameter()]
    [switch]$Restore,
    
    [Parameter()]
    [switch]$Build,
    
    [Parameter()]
    [switch]$Publish
)

$ErrorActionPreference = 'Stop'
$SolutionFile = Join-Path $PSScriptRoot 'Strat.slnx'
$OutputDir = Join-Path $PSScriptRoot 'artifacts'

function Write-Header {
    param([string]$Message)
    Write-Host ""
    Write-Host "========================================" -ForegroundColor Cyan
    Write-Host " $Message" -ForegroundColor Cyan
    Write-Host "========================================" -ForegroundColor Cyan
}

# Default: if no switches specified, do Restore + Build
if (-not ($Clean -or $Restore -or $Build -or $Publish)) {
    $Restore = $true
    $Build = $true
}

if ($Clean) {
    Write-Header "Cleaning..."
    if (Test-Path $OutputDir) {
        Remove-Item -Recurse -Force $OutputDir
    }
    dotnet clean $SolutionFile -c $Configuration --nologo -v q
    Write-Host "Clean completed." -ForegroundColor Green
}

if ($Restore) {
    Write-Header "Restoring packages..."
    dotnet restore $SolutionFile --nologo
    Write-Host "Restore completed." -ForegroundColor Green
}

if ($Build) {
    Write-Header "Building ($Configuration)..."
    dotnet build $SolutionFile -c $Configuration --no-restore --nologo
    Write-Host "Build completed." -ForegroundColor Green
}

if ($Publish) {
    Write-Header "Publishing ($Configuration)..."
    $PublishDir = Join-Path $OutputDir 'publish'
    dotnet publish (Join-Path $PSScriptRoot 'src/Host/Strat.Host/Strat.Host.csproj') `
        -c $Configuration `
        -o $PublishDir `
        --no-restore `
        --nologo
    Write-Host "Publish completed. Output: $PublishDir" -ForegroundColor Green
}

Write-Host ""
Write-Host "All tasks completed successfully!" -ForegroundColor Green
