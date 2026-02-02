#!/usr/bin/env pwsh
# Build script for Strat Client

param(
    [Parameter()]
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release',
    
    [Parameter()]
    [ValidateSet('Desktop', 'Browser', 'All')]
    [string]$Target = 'Desktop',
    
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
$DesktopProject = Join-Path $PSScriptRoot 'src/06.Hosts/Strat.Desktop/Strat.Desktop.csproj'
$BrowserProject = Join-Path $PSScriptRoot 'src/06.Hosts/Strat.Browser/Strat.Browser.csproj'

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
    if ($Target -eq 'Desktop' -or $Target -eq 'All') {
        Write-Header "Publishing Desktop ($Configuration)..."
        $PublishDir = Join-Path $OutputDir 'desktop'
        dotnet publish $DesktopProject `
            -c $Configuration `
            -o $PublishDir `
            --no-restore `
            --nologo
        Write-Host "Desktop published to: $PublishDir" -ForegroundColor Green
    }
    
    if ($Target -eq 'Browser' -or $Target -eq 'All') {
        Write-Header "Publishing Browser ($Configuration)..."
        $PublishDir = Join-Path $OutputDir 'browser'
        dotnet publish $BrowserProject `
            -c $Configuration `
            -o $PublishDir `
            --no-restore `
            --nologo
        Write-Host "Browser published to: $PublishDir" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "All tasks completed successfully!" -ForegroundColor Green
